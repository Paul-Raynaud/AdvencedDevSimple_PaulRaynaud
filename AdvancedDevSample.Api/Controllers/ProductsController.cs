﻿using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des produits
    /// </summary>
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    [Authorize]  // Sécurise toutes les routes de ce contrôleur
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Créer un nouveau produit
        /// </summary>
        /// <param name="request">Données du produit à créer</param>
        /// <returns>Le produit créé</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), 201)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                var result = _productService.CreateProduct(request);
                return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtenir un produit par son ID
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Le produit demandé</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(Guid id)
        {
            try
            {
                var result = _productService.GetProduct(id);
                return Ok(result);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtenir tous les produits
        /// </summary>
        /// <returns>Liste de tous les produits</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), 200)]
        public IActionResult GetAllProducts()
        {
            var results = _productService.GetAllProducts();
            return Ok(results);
        }

        /// <summary>
        /// Mettre à jour un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <param name="request">Données de mise à jour</param>
        /// <returns>Le produit mis à jour</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                var result = _productService.UpdateProduct(id, request);
                return Ok(result);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Supprimer un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Confirmation de suppression</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Changer le prix d'un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <param name="request">Nouveau prix</param>
        /// <returns>Confirmation du changement</returns>
        [HttpPut("{id}/price")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                _productService.ChangePrice(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
