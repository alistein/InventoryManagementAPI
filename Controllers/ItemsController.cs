using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AllRolesOnly")]
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ItemsController(IUnitOfWork unitOfWork, IMapper mapper) {_unitOfWork = unitOfWork; _mapper = mapper; }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetItems()
        {
            return await _unitOfWork.ItemRepository.GetAllData().Include(i => i.Sales).Select(it => new
            {
                it.ID,
                it.Name,
                it.Price,
                it.Description,
                it.Quantity,
                Sales = it.Sales!.Select(s => new
                {
                    s.ID,
                    s.ItemId,
                    s.QuantitySold,
                    s.DateOfSale
                })

            }).ToListAsync();
        }


        [HttpGet("onlyItems")]
        public async Task<ActionResult<IEnumerable<GetItemDto>>> GetOnlyItems()
        {
            var items = await _unitOfWork.ItemRepository.GetAllList();

            var itemsFiltered = _mapper.Map<IEnumerable<GetItemDto>>(items);

            return Ok(itemsFiltered);
        }


        [HttpGet("onlyItemSingle/{id}")]
        public async Task<ActionResult<GetItemDto>> GetOnlySingleItem(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetSingleData(id);

            var itemFiltered = _mapper.Map<GetItemDto>(item);

            return itemFiltered;
        }


        [HttpGet("single/{id}")]
        public async Task<ActionResult<object>> GetItem(int id)
        {
            var transformedList = await _unitOfWork.ItemRepository.GetAllData().Include(i => i.Sales).Select(it => new
            {
                it.ID,
                it.Name,
                it.Price,
                it.Description,
                it.Quantity,
                Sales = it.Sales!.Select(s => new
                {
                    s.ID,
                    s.ItemId,
                    s.QuantitySold,
                    s.DateOfSale
                })

            }).ToListAsync();

            var selectedItem = transformedList.FirstOrDefault(t => t.ID == id);

            if (selectedItem is null)
            {
                return NotFound();
            }


            return selectedItem;

        }
        // Comment
        [Authorize(Policy = "MixEditorUserOrAdmin")]
        [HttpPost("add")]
        public async Task<ActionResult<CreateItemDTO>> AddItem(CreateItemDTO AddDto)
        {

            var itemDto = _mapper.Map<ItemModel>(AddDto);

            await _unitOfWork.ItemRepository.AddData(itemDto);

            await _unitOfWork.Commit();

            return CreatedAtAction(nameof(GetItem), new { id = itemDto.ID }, itemDto);


        }

        [Authorize(Policy = "MixEditorUserOrAdmin")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditItem(int id, [FromBody]CreateItemDTO dto)
        {
            var selectedItemForEdit = await _unitOfWork.ItemRepository.Find(id);

            if(selectedItemForEdit is null)
            {
                return NotFound();
            }

            selectedItemForEdit.Name = dto.Name;
            selectedItemForEdit.Description = dto.Description;
            selectedItemForEdit.Quantity = dto.Quantity;
            selectedItemForEdit.Price = dto.Price;

             _unitOfWork.ItemRepository.EditData(selectedItemForEdit);

             await _unitOfWork.Commit();

            return NoContent();
            
        }
        [Authorize(Policy = "MixEditorUserOrAdmin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var selectedItemForDelete = await _unitOfWork.ItemRepository.Find(id);

            if(selectedItemForDelete is null)
            {
                return NotFound();
            }

            _unitOfWork.ItemRepository.RemoveData(selectedItemForDelete);

            await _unitOfWork.Commit();

            return NoContent();
        }

    }
}