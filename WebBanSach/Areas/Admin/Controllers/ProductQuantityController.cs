using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Globalization;
using System;
using System.IO;
using WebBanSach.Application.Implementation;
using WebBanSach.Application.Interfaces;
using WebBanSach.Utilities.Helpers;
using WebBanSach.Data.Entities;

namespace WebBanSach.Areas.Admin.Controllers
{
	public class ProductQuantityController : BaseController
	{
		public IProductQuantityService _productQuantityService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductQuantityController(IProductQuantityService productQuantityService, IWebHostEnvironment hostingEnvironment)
		{
			_productQuantityService = productQuantityService;
            _hostingEnvironment = hostingEnvironment;

        }

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAll()
		{
			var model = _productQuantityService.GetAll();

			return new OkObjectResult(model);
		}

		[HttpGet]
		public IActionResult GetById(int id)
		{
			var model = _productQuantityService.GetById(id);

			return new OkObjectResult(model);
		}

		[HttpGet]
		public IActionResult GetAllPaging(string startDate, string endDate, string keyword, int page, int pageSize)
		{
			var model = _productQuantityService.GetAllPaging(startDate, endDate, keyword, page, pageSize);
			return new OkObjectResult(model);
		}

        [HttpPost]
        public IActionResult ExportExcel(string publisher, string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            Guid guid= Guid.NewGuid();
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"Phieu_Nhap_{guid}.xlsx";
            // Template File
            string templateDocument = Path.Combine(sWebRootFolder, "templates", "Phieu_Nhap.xlsx");

            string url = $"{Request.Scheme}://{Request.Host}/{"export-files"}/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["WebBanSachImport"];
                    // Data Acces, load order header data.

                    // Insert customer data into template
                    worksheet.Cells[3, 1].Value = "Ngày nhập: " + start.Day + "/" + start.Month + "/" + start.Year;
                    worksheet.Cells[4, 1].Value = "Nhà cung cấp: " + publisher;
                    
                    // Start Row for Detail Rows
                    int rowIndex = 9;

                    // load order details
                    var importDetails = _productQuantityService.GetImport(publisher, startDate, endDate);
                    int count = 1;
                    int importQuantity = 0;
                    decimal importPrice = 0;
                    decimal total = 0;
                    foreach (var importDetail in importDetails)
                    {
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();

                        worksheet.Cells[rowIndex, 2].Value = importDetail.Product.Name;

                        worksheet.Cells[rowIndex, 3].Value = importDetail.Quantity.ToString();
                        importQuantity = importDetail.Quantity;

                        worksheet.Cells[rowIndex, 4].Value = importDetail.Product.Price.ToString("N0");
                        importPrice = importDetail.Product.Price;

                        worksheet.Cells[rowIndex, 5].Value = (importDetail.Product.Price * importDetail.Quantity).ToString("N0");

                        total += importQuantity * importPrice;
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    worksheet.Cells[24, 3].Value = importQuantity.ToString();
                    worksheet.Cells[24, 5].Value = total.ToString("N0");

                    var numberWord = "Tổng tiền (chữ): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;

                    worksheet.Cells[28, 3].Value = start.Day + ", " + start.Month + ", " + start.Year;


                    package.SaveAs(file); //Save the workbook.
                }
            }
            return new OkObjectResult(url);
        }
    }
}
