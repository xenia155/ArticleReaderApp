using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace webapi
{
    public interface IEntityService
    {
        public List<Entity> ExtractEntities(Article a);
    }

    public class EntityExtractor : IEntityService
    {
        private readonly ILogger<EntityExtractor> _logger;
        private readonly MyApplicationContext _dbContext;

        public EntityExtractor(ILogger<EntityExtractor> logger, MyApplicationContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<Entity> ExtractEntities(Article a)
        {
            try
            {
                // Initialize the Pullenti library
                Pullenti.Sdk.InitializeAll();

                // Create a list to store the extracted entities
                List<Entity> entities = new List<Entity>();

                // Log the input text
                _logger.LogInformation($"Input text: {a.Text}");

                // Create a processor for the text using the named entity recognizer analyzer
                using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateSpecificProcessor(Pullenti.Ner.Named.NamedEntityAnalyzer.ANALYZER_NAME))
                {
                    // Process the text and get the analysis result
                    Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(a.Text), null, null);

                    // Log the number of entities found in the analysis result
                    _logger.LogInformation($"Found {ar.Entities.Count} entities in the analysis result");

                    // Loop through the entities in the analysis result
                    foreach (Pullenti.Ner.Referent e in ar.Entities)
                    {
                        // Check if the entity is a named entity
                        if (e is Pullenti.Ner.Named.NamedEntityReferent)
                        {
                            // Cast the named entity to a NamedEntityReferent object
                            Pullenti.Ner.Named.NamedEntityReferent ne = e as Pullenti.Ner.Named.NamedEntityReferent;

                            // Get the text and type of the entity
                            string entityText = ne.GetStringValue(Pullenti.Ner.Named.NamedEntityReferent.ATTR_NAME);
                            string entityType = ne.TypeName;

                            // Check if the entity already exists in the database
                            Entity existingEntity = _dbContext.Entities.FirstOrDefault(en => en.Text == entityText);


                            if (existingEntity == null)
                            {
                                // Create a new Entity object and set its properties
                                Entity entity = new Entity
                                {
                                    Text = entityText,
                                    Type = entityType,
                                    ArticleId = a.ArticleId,
                                    Article = a,
                                };

                                // Add the Entity object to the list of entities
                                entities.Add(entity);

                                // Add the Entity object to the entities table
                                _dbContext.Entities.Add(entity);
                            }
                            else
                            {
                                // Associate the existing entity with the current article
                                existingEntity.ArticleId = a.ArticleId;
                                existingEntity.Article = a;
                            }
                        }
                    }

                    // Save changes to the database
                    _dbContext.SaveChanges();
                }

                // Log the success message and return the list of entities
                _logger.LogInformation("Successfully extracted entities");
                Console.WriteLine("Extracted entities:");
                foreach (var entity in entities)
                {
                    Console.WriteLine($"{entity.Type}: {entity.Text}");
                }
                return entities;
            }
            catch (Exception ex)
            {
                // Log the error message and throw the exception
                _logger.LogError(ex, "An error occurred while extracting entities");
                throw;
            }
        }
    }
}
