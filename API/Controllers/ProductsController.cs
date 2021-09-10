using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Core.Specifications;
using AutoMapper;
using API.Dtos;
using Core.Params;
using Core.Pagination;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> repoProduct;
        private readonly IGenericRepository<ProductType> repoType;
        private readonly IGenericRepository<ProductBrand> repoBrand;

        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> repoProduct,
            IGenericRepository<ProductType> repoType,
            IGenericRepository<ProductBrand> repoBrand,
            IMapper mapper)
        {
            this.repoProduct = repoProduct;
            this.repoType = repoType;
            this.repoBrand = repoBrand;
            this.mapper = mapper;
        }

        [HttpGet]
        [Cached(600)]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> GetProducts(
            [FromQuery] ProductParams productParams
        )
        {
            // Pagination Products
            var countSpec = new ProductWithFiltersForCountSpecificication(productParams);

            var count = await repoProduct.CountAsync(countSpec);

            // Get Products
            ProductBrandTypesSpecifications specifications = new(productParams);

            var products = await repoProduct.GetAllWithSpec(specifications);

            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductReturnDto>>(products);

            return Ok(new Pagination<ProductReturnDto>(productParams.PageIndex,
                productParams.PageSize, count, data));
        }

        [HttpGet("{id}")]
        [Cached(600)]
        public async Task<ActionResult<ProductReturnDto>> GetProduct(int id)
        {
            var specifications = new ProductBrandTypesSpecifications(id);

            var product = await repoProduct.GetEntityWithSpec(specifications);

            return mapper.Map<ProductReturnDto>(product);
        }

        [HttpGet("types")]
        [Cached(600)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await repoType.GetAllAsync());
        }

        [HttpGet("brands")]
        [Cached(600)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await repoBrand.GetAllAsync());
        }
    }
}