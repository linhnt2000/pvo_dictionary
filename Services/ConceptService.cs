using System;
using System.Linq;
using AutoMapper;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Models.Request;
using thu6_pvo_dictionary.Repositories;

namespace thu6_pvo_dictionary.Services
{
    public class ConceptService
    {
        private readonly UserRepository _userRepository;
        private readonly DictionaryRepository _dictionaryRepository;
        private readonly ConceptRepository _conceptRepository;
        private readonly ExampleRepository _exampleRepository;
        private readonly ExampleRelationshipRepository _exampleRelationshipRepository;
        private readonly ConceptRelationshipRepository _conceptRelationshipRepository;
        private readonly ConceptLinkRepository _conceptLinkRepository;
        private readonly ApiOption _apiOption;
        private readonly IMapper _mapper;
        private readonly AppDbContext _databaseContext;
        public ConceptService(ApiOption apiOption, AppDbContext databaseContext, IMapper mapper)
        {
            _userRepository = new UserRepository(apiOption, databaseContext, mapper);
            _dictionaryRepository = new DictionaryRepository(apiOption, databaseContext, mapper);
            _conceptRepository = new ConceptRepository(apiOption, databaseContext, mapper);
            _exampleRepository = new ExampleRepository(apiOption, databaseContext, mapper);
            _exampleRelationshipRepository = new ExampleRelationshipRepository(apiOption, databaseContext, mapper);
            _conceptRelationshipRepository = new ConceptRelationshipRepository(apiOption, databaseContext, mapper);
            _conceptLinkRepository = new ConceptLinkRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
            _databaseContext = databaseContext;
        }

        public object GetListConcept(int dictionaryId)
        {
            try
            {
                var dictionary = _dictionaryRepository.GetById(dictionaryId);
                if (dictionary == null)
                {
                    throw new Exception("Dictionary does not exist!");
                }

                var conceptList = _conceptRepository.GetEntitiesByCondition(row => row.dictionary_id == dictionaryId).ToList();
                return new
                {
                    ListConcept = conceptList

                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object AddConcept(AddConceptRequest addConceptRequest)
        {
            try
            {
                var dictionary = _dictionaryRepository.FindEntityById(addConceptRequest.DictionaryId);
                if (dictionary == null)
                {
                    throw new ValidateError(3001, "Dictionary does not exist!");
                }
   
             

                var newConcept = new Concept()
                {
                    dictionary_id = addConceptRequest.DictionaryId,
                    title = addConceptRequest.Title,
                    description = addConceptRequest.Description,
                };
                _conceptRepository.CreateEntity(newConcept);
                _conceptRepository.SaveChange();

                // update dictionary
                dictionary.updated_date = DateTime.Now;
                _dictionaryRepository.UpdateEntity(dictionary);
                _dictionaryRepository.SaveChange();

                return newConcept;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <returns></returns>
        public object UpdateConcept(UpdateConceptRequest request)
        {
            try
            {
                var concept = _conceptRepository.FindEntityById(request.ConceptId);
                if (concept == null)
                {
                    throw new Exception("Concept Id invalid!");
                }

                var conceptByTitleNumber = _conceptRepository.GetEntitiesByCondition(row => row.concept_id != concept.concept_id && row.title == request.Title).Count();
                if (conceptByTitleNumber > 0)
                {
                    throw new ValidateError(3001, "Concept already exists");
                }

                concept.title = request.Title;
                if (!string.IsNullOrEmpty(request.Description))
                {
                    concept.description = request.Description;
                }
                concept.updated_date = DateTime.Now;
                _conceptRepository.UpdateEntity(concept);
                _conceptRepository.SaveChange();

                return concept;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object DeleteConcept(int conceptId, bool? isForced)
        {
            try
            {
                var concept = _conceptRepository.FindEntityById(conceptId);
                if (concept == null)
                {
                    throw new Exception("Concept Id invalid");
                }

                if (!isForced.HasValue || !isForced.Value)
                {
                    // Check if the concept has associated examples
                    var hasExamples = _databaseContext.example_relationships.Any(e => e.concept_id == conceptId);
                    if (hasExamples)
                    {
                        throw new ValidateError(3002, "Concept can't be deleted: Concept is linked to examples and cannot be deleted.");
                    }
                }

                _conceptRepository.DeleteEntity(concept);
                _conceptRepository.SaveChange();

                return new
                {
                    Status = 1
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetConcept(int conceptId)
        {
            try
            {
                var concept = _conceptRepository.FindEntityById(conceptId);
                if (concept == null)
                {
                    throw new Exception("Concept Id invalid");
                }

                var exampleRelationshipList = _exampleRelationshipRepository.GetEntitiesByCondition(row => row.concept_id == concept.concept_id).ToList();
                var exampleIdList = exampleRelationshipList.Select(row => row.example_id).ToList();
                var exampleList = _exampleRepository.GetEntitiesByCondition(row => exampleIdList.Contains(row.example_id)).ToList();

                return new
                {
                    Title = concept.title,
                    Description = concept.description,
                    ListExample = exampleList
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetConceptRelationship(int conceptId, int parentId)
        {
            try
            {
                var conceptRelationships = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.concept_id == conceptId && row.parent_id == parentId).ToList();
                if (conceptRelationships == null || conceptRelationships.Count == 0)
                {
                    throw new Exception("ConceptRelationships do not exist!");
                }

                var conceptLinks = _conceptLinkRepository.GetEntitiesByCondition(link => conceptRelationships.Select(cr => cr.concept_link_id).Contains(link.concept_link_id)).ToList();

                var result = conceptLinks.Select(cl => new
                {
                    ConceptLinkId = cl.concept_link_id,
                    ConceptLinkName = cl.concept_link_name
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object UpdateConceptRelationship(UpdateConceptRelationshipReuqest request)
        {
            try
            {
                if (request.conceptId == request.parentId)
                {
                    throw new ValidateError(3003, "A concept can't link to itself!");
                }

                var concept = _conceptRepository.FindEntityById(request.conceptId);
                if (concept == null)
                {
                    throw new ValidateError(2000, "Concept does not exist!");
                }

                var parent = _conceptRepository.FindEntityById(request.parentId);
                if (parent == null)
                {
                    throw new ValidateError(2000, "Concept parent does not exist!");
                }

                var conceptRelationship = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.concept_id == request.conceptId && row.parent_id == request.parentId).FirstOrDefault();
                if (conceptRelationship == null)
                {
                    conceptRelationship = new ConceptRelationship()
                    {
                        concept_id = request.conceptId,
                        parent_id = request.parentId,
                        concept_link_id = request.conceptLinkId,
                        dictionary_id = parent.dictionary_id,
                    };

                    _conceptRelationshipRepository.CreateEntity(conceptRelationship);
                    _conceptRelationshipRepository.SaveChange();

                    return conceptRelationship;
                }

                conceptRelationship.concept_link_id = request.conceptLinkId;
                conceptRelationship.updated_date = DateTime.Now;
                _conceptRelationshipRepository.UpdateEntity(conceptRelationship);
                _conceptRelationshipRepository.SaveChange();
                return conceptRelationship;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetTree(int conceptId, string? conceptName)
        {
            try
            {
                var conceptRelationshipParentList = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.concept_id == conceptId).ToList();
                var conceptParentIdList = conceptRelationshipParentList.Select(row => row.parent_id).ToList();
                var listParent = _conceptRepository.GetEntitiesByCondition(row => conceptParentIdList.Contains(row.concept_id)).ToList();

                var conceptRelationshipChildrenList = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.parent_id == conceptId).ToList();
                var conceptChildrenIdList = conceptRelationshipChildrenList.Select(row => row.concept_id).ToList();
                var listChildren = _conceptRepository.GetEntitiesByCondition(row => conceptChildrenIdList.Contains(row.concept_id)).ToList();

                return new
                {
                    ListParent = listParent,
                    ListChildren = listChildren,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetConceptParents(int conceptId)
        {
            try
            {
                var conceptRelationshipParentList = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.concept_id == conceptId).ToList();
                var conceptParentIdList = conceptRelationshipParentList.Select(row => row.parent_id).ToList();
                var listParent = _conceptRepository.GetEntitiesByCondition(row => conceptParentIdList.Contains(row.concept_id)).ToList();

                return listParent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetConceptChildren(int conceptId)
        {
            try
            {
                var conceptRelationshipChildrenList = _conceptRelationshipRepository.GetEntitiesByCondition(row => row.parent_id == conceptId).ToList();
                var conceptChildrenIdList = conceptRelationshipChildrenList.Select(row => row.concept_id).ToList();
                var listChildren = _conceptRepository.GetEntitiesByCondition(row => conceptChildrenIdList.Contains(row.concept_id)).ToList();

                return listChildren;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
