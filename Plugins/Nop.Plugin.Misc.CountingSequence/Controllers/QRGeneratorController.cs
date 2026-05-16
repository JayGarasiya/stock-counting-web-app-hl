using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Controllers;
using QRCoder;
using System.Text;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class QRGeneratorController : BaseAdminController
    {
        #region Fields

        protected readonly IProductService _productService;
        protected readonly IPictureService _pictureService;
        protected readonly ICategoryService _categoryService;
        protected readonly ISpecificationAttributeService _specificationAttributeService;
        protected readonly IWarehouseService _warehouseService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public QRGeneratorController(
            IProductService productService,
            IPictureService pictureService,
            ICategoryService categoryService,
            ISpecificationAttributeService specificationAttributeService,
            IWarehouseService warehouseService,
            ILocalizationService localizationService)
        {
            _productService = productService;
            _pictureService = pictureService;
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
            _warehouseService = warehouseService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return Json(false);

            var categories = await _categoryService.GetProductCategoriesByProductIdAsync(productId);
            var warehouseInventory = await _productService.GetAllProductWarehouseInventoryRecordsAsync(productId);

            var sb = new StringBuilder();
            sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.Name")}: {product.Name},");
            sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.Sku")}: {product.Sku},");
            
            var categoryNames = new List<string>();

            foreach (var cat in categories)
            {
                var category = await _categoryService.GetCategoryByIdAsync(cat.CategoryId);

                if (category != null)
                    categoryNames.Add(category.Name);
            }

            sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.Categories")}: {string.Join(", ", categoryNames)},");
            
            var specs = await _specificationAttributeService.GetProductSpecificationAttributesAsync(productId);

            foreach (var spec in specs)
            {
                var option = await _specificationAttributeService.GetSpecificationAttributeOptionByIdAsync(spec.SpecificationAttributeOptionId);

                if (option != null)
                {
                    var specificationAttribute = await _specificationAttributeService.GetSpecificationAttributeByIdAsync(option.SpecificationAttributeId);
                    sb.AppendLine($"{specificationAttribute?.Name}: {option.Name},");
                }
            }
            sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.OrderMaximumQuantity")}: {product.OrderMaximumQuantity},");
            foreach (var wh in warehouseInventory)
            {
                var warehouse = await _warehouseService.GetWarehouseByIdAsync(wh.WarehouseId);
                sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.Warehouse")}: {warehouse?.Name},");
                sb.AppendLine($"{await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.StockQuantity")}: {wh.StockQuantity}");
            }
            string qrText = sb.ToString();

            var productPictures = await _productService.GetProductPicturesByProductIdAsync(productId);
            foreach (var productPicture in productPictures)
            {
                var picture = await _pictureService.GetPictureByIdAsync(productPicture.PictureId);

                if (picture != null)
                {
                    await _productService.DeleteProductPictureAsync(productPicture);
                    await _pictureService.DeletePictureAsync(picture);
                }
            }

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                var picture = await _pictureService.InsertPictureAsync(
                    qrCodeImage,
                    "image/png",
                    $"product_{productId}_qr"
                );

                var productPicture = new ProductPicture
                {
                    ProductId = productId,
                    PictureId = picture.Id,
                    DisplayOrder = 0
                };

                await _productService.InsertProductPictureAsync(productPicture);
            }

            return Json(new
            {
                success = true,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Product.QRCodeNotification")
            });
        }

        #endregion
    }
}
