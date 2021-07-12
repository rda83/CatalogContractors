using Microsoft.AspNetCore.Mvc;
using CatalogContractors.Models.Repositories;
using CatalogContractors.Models;
using CatalogContractors.API.ViewModel;
using CatalogContractors.Infra;
using Microsoft.AspNetCore.Http;

namespace CatalogContractors.API.Controllers
{
    /// <summary>
    /// Справочник типы контактной информации
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TypesContactController : ControllerBase
    {

        private ITypeContactRepository _typeContactRepository;

        public TypesContactController(ITypeContactRepository typeContactRepository)
        {
            _typeContactRepository = typeContactRepository;
        }

        /// <summary>
        /// Добавление элемента: Тип контактной информации
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TypesContact
        ///     {
        ///         "Name": "e-mail"
        ///    }
        ///
        /// </remarks>
        /// <returns>Результат выполнения операции</returns>
        /// <response code="200">Результат сохранения объекта в справочнике</response>
        /// <response code="400">При неудачном выполнении запроса</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseViewModel> PostTypesContact(TypeContact typeContact)
        {
            _typeContactRepository.Add(typeContact);
            var result = _typeContactRepository.Save();
            return ResponseViewModel.GetResponseViewModel(result, typeContact.Id);
        }

        /// <summary>
        /// Возвращает содержимое справочника: Типы контактной информации
        /// </summary>
        [HttpGet(Name = nameof(GetAllTypesContact))]
        public IActionResult GetAllTypesContact([FromQuery] QueryParameters queryParameters)
        {

            var result = _typeContactRepository.GetAll(queryParameters);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает элемент: Тип контактной информации - по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор типа контактной информации</param>
        [HttpGet("{id}")]
        public ActionResult<TypeContact> GetTypesContact(long id)
        {
            var typeContact = _typeContactRepository.GetById(id);

            if (typeContact == null)
            {
                return NotFound();
            }

            return typeContact;
        }

        /// <summary>
        /// Корректировка элемента: Тип контактной информации
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /TypesContact
        ///     {
        ///         "id": 1,
        ///         "Name": "e-mail",
        ///    }
        ///
        /// </remarks>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="200">Результат сохранения изменений в справочнике</response>
        /// <response code="400">При неудачном выполнении запроса</response>
        [HttpPatch]
        public ActionResult<ResponseViewModel> PatchTypesContact([FromBody] TypeContact typeContact)
        {

            if (!_typeContactRepository.Update(typeContact))
            {
                return NotFound();
            }
            
            var result = _typeContactRepository.Save();

            return ResponseViewModel.GetResponseViewModel(result, typeContact.Id);
        }

        /// <summary>
        /// Удаление элемента: Тип контактной информации
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ResponseViewModel> DeleteTypesContact(long id)
        {
            if (!_typeContactRepository.Delete(id))
            {
                return NotFound();
            }

            var result = _typeContactRepository.Save();

            return ResponseViewModel.GetResponseViewModel(result, id);
        }
    }
}
