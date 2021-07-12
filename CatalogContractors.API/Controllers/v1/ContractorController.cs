using Microsoft.AspNetCore.Mvc;
using CatalogContractors.Models.Repositories;
using CatalogContractors.Models;
using CatalogContractors.API.ViewModel;
using CatalogContractors.Infra;
using Microsoft.AspNetCore.Http;

namespace CatalogContractors.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContractorController : ControllerBase
    {
        private IContractorRepository _сontractorRepository;
        private IContactRepository _contactRepository;

        public ContractorController(IContractorRepository сontractorRepository, IContactRepository contactRepository)
        {
            _сontractorRepository = сontractorRepository;
            _contactRepository = contactRepository;
        }

        #region Contractors

            /// <summary>
            /// Добавление элемента справочника контрагенты
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     POST /Contractor
            ///     {
            ///         "Name": "Partner 1",
            ///         "Description": "Partner 1 desc",
            ///         "Contacts": [
            ///             {
            ///                 "value": "mail@mail.com"
            ///             },
            ///             {
            ///                 "value": "+1555888456456"
            ///             }
            ///         ]
            ///    }
            ///
            /// </remarks>
            /// <returns>Результат выполнения операции</returns>
            /// <response code="200">Результат сохранения объекта в справочнике</response>
            /// <response code="400">При неудачном выполнении запроса</response>  
            [HttpPost]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public ActionResult<ResponseViewModel> PostContractor(Contractor сontractor)
                {
                    _сontractorRepository.Add(сontractor);
                    var result = _сontractorRepository.Save();
                    return ResponseViewModel.GetResponseViewModel(result, сontractor.Id);
                }

            /// <summary>
            /// Возвращает содержимое справочника контрагенты
            /// </summary>
            [HttpGet(Name = nameof(GetAllContractor))]
            public IActionResult GetAllContractor([FromQuery] QueryParameters queryParameters)
            {
                var result = _сontractorRepository.GetAll(queryParameters);
                return Ok(result);
            }


            /// <summary>
            /// Возвращает элемент справочника контрагенты
            /// </summary>
            /// <param name="id">Идентификатор элемента</param>
            [HttpGet("{id}")]
            public ActionResult<Contractor> GetContractor(long id)
            {
                var сontractor = _сontractorRepository.GetById(id);

                if (сontractor == null)
                {
                    return NotFound();
                }

                return сontractor;
            }

            /// <summary>
            /// Корректировка элемента справочника контрагенты
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     PATCH /Contractor
            ///     {
            ///         "id": 1,
            ///         "Name": "e-mail",
            ///    }
            ///
            /// </remarks>
            /// <returns>Результат выполнения операции</returns>
            /// <response code="200">Результат сохранения изменений в справочнике</response>
            /// <response code="400">При неудачном выполнении запроса</response>
            [HttpPatch]
            public ActionResult<ResponseViewModel> PatchContractor([FromBody] Contractor contractor)
            {

                if (!_сontractorRepository.Update(contractor))
                {
                    return NotFound();
                }
            
                var result = _сontractorRepository.Save();

                return ResponseViewModel.GetResponseViewModel(result, contractor.Id);
            }

            /// <summary>
            /// Удаление элемента справочника контрагент
            /// </summary>
            [HttpDelete("{id}")]
            public ActionResult<ResponseViewModel> DeleteContractor(long id)
            {
                if (!_сontractorRepository.Delete(id))
                {
                    return NotFound();
                }

                var result = _сontractorRepository.Save();

                return ResponseViewModel.GetResponseViewModel(result, id);
            }

        #endregion

        #region Contractors/Contacts

            /// <summary>
            /// Добавление элемента контактной информации для контрагента
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     POST /Contractor/{contractorId}/Contacts
            ///         {
            ///             "value": "+1555888456456"
            ///         }
            ///
            /// </remarks>
            /// <returns>Результат выполнения операции</returns>
            /// <response code="200">Результат сохранения объекта в справочнике</response>
            /// <response code="400">При неудачном выполнении запроса</response>  
            [HttpPost("{contractorId}/Contacts", Name = nameof(PostContractorContact))]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public ActionResult<ResponseViewModel> PostContractorContact(long contractorId, Contact contact)
                {
                    _contactRepository.Add(contractorId, contact);
                    var result = _contactRepository.Save();
                    return ResponseViewModel.GetResponseViewModel(result, contact.Id);
                }

            /// <summary>
            /// Возвращает все элементы контактной информации для контрагента
            /// </summary>
            [HttpGet("{contractorId}/Contacts", Name = nameof(GetAllContactsForContractor))]
            public IActionResult GetAllContactsForContractor(long contractorId, [FromQuery] QueryParameters queryParameters)
            {
                var result = _contactRepository.GetAll(contractorId, queryParameters);
                return Ok(result);
            }

            /// <summary>
            /// Возвращает указанный элемент контактной информации для контрагента
            /// </summary>
            /// <param name="id">Идентификатор элемента</param>
            [HttpGet("{contractorId}/Contacts/{id}", Name = nameof(GetContact))]
            public ActionResult<Contact> GetContact(long id)
            {

                //fixme: получаем любой контакт по id, без проверки принадлежности к родителю

                var contact = _contactRepository.GetById(id);

                if (contact == null)
                {
                    return NotFound();
                }

                return contact;
            }

            /// <summary>
            /// Корректировка элемента контактной информации контрагента
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     PATCH /Contractor/{contractorId}/Contacts
            ///     {
            ///         "id": 1,
            ///         "value": "mail@mail.com"
            ///    }
            ///
            /// </remarks>
            /// <returns>Результат выполнения операции</returns>
            /// <response code="200">Результат сохранения изменений в справочнике</response>
            /// <response code="400">При неудачном выполнении запроса</response>
            [HttpPatch("{contractorId}/Contacts", Name = nameof(PatchContact))]
            public ActionResult<ResponseViewModel> PatchContact([FromBody] Contact contact)
            {

                if (!_contactRepository.Update(contact))
                {
                    return NotFound();
                }

                var result = _contactRepository.Save();

                return ResponseViewModel.GetResponseViewModel(result, contact.Id);
            }

            /// <summary>
            /// Удалет элемент контактной информации для контрагента
            /// </summary>
            [HttpDelete("{contractorId}/Contacts/{id}", Name = nameof(DeleteContact))]
            public ActionResult<ResponseViewModel> DeleteContact(long id)
            {
                if (!_contactRepository.Delete(id))
                {
                    return NotFound();
                }

                var result = _contactRepository.Save();

                return ResponseViewModel.GetResponseViewModel(result, id);
            }

        #endregion
    }
}
