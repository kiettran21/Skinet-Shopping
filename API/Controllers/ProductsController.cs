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
        public async Task<ActionResult<IReadOnlyList<ProductReturnDto>>> GetProducts()
        {
            ProductBrandTypesSpecifications specifications = new ProductBrandTypesSpecifications();

            var products = await repoProduct.GetAllWithSpec(specifications);

            return Ok(mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReturnDto>> GetProduct(int id)
        {
            var specifications = new ProductBrandTypesSpecifications(id);

            var product = await repoProduct.GetEntityWithSpec(specifications);

            return mapper.Map<ProductReturnDto>(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await repoType.GetAllAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await repoBrand.GetAllAsync());
        }
    }
}