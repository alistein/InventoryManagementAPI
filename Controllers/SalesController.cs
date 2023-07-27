
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data.UnitOfWork;
using InventoryManagementSystem.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "MixAdminAndEditor")]

    public class SalesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }


        [HttpGet("all")]
        public async Task<IEnumerable<GetSaleDto>> GetAllSales()
        {
            IEnumerable<SaleModel> sales = await _unitOfWork.SaleRepository.GetAllList();

            IEnumerable<GetSaleDto> saleDtos = _mapper.Map<IEnumerable<GetSaleDto>>(sales);

            return saleDtos;


        }


        [HttpGet("single/{id}")]
        public async Task<ActionResult<GetSaleDto>> GetSingleSale(int id)
        {
            SaleModel? selectedSale = await _unitOfWork.SaleRepository.GetSingleData(id);

            if (selectedSale is null)
            {
                return NotFound();
            }

            GetSaleDto saleDto = _mapper.Map<GetSaleDto>(selectedSale);

            return Ok(saleDto);
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<object>>> GetReports()
        {
            var reports = await _unitOfWork.ItemRepository.GetAllData()
                .Select(g => new {
                
                    ItemId = g.ID,
                    ItemName = g.Name,
                    CountOfAllSoldItems = g.Sales!.Count(),
                    AllSoldItems = g.Sales!.Sum(s => s.QuantitySold)
                }).ToListAsync();



            return reports;
        }


        [HttpGet("singleReport/{id}")]
        public async Task<ActionResult<object>> GetSingleReport(int id)
        {
            var reports = await _unitOfWork.ItemRepository.GetAllData()
                      .Select(g => new {

                          ItemId = g.ID,
                          ItemName = g.Name,
                          CountOfAllSoldItems = g.Sales!.Count(),
                          AllSoldItems = g.Sales!.Sum(s => s.QuantitySold)
                      }).ToListAsync();

            var singleReport = reports.Where(sr => sr.ItemId == id);


            return (object) singleReport;
        }

        [Authorize(Policy = "EditorAdminOnly")]
        [HttpPost("add")]
        public async Task<ActionResult<SaleModel>> AddSale(CreateSaleDTO saleDto)
        {
            var sale = _mapper.Map<SaleModel>(saleDto);

            await _unitOfWork.SaleRepository.AddData(sale);

            await _unitOfWork.Commit();

            return CreatedAtAction(nameof(GetSingleSale), new { id = sale.ID }, sale);


        }

    }
}