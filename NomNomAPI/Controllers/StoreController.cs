using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NomNomAPI.Models;
using NomNomAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    // GET: /Store
    [HttpGet]
    public async Task<ActionResult<List<Store>>> GetAllStores()
    {
        var stores = await _storeService.GetAllStores();
        if (stores == null || stores.Count == 0)
        {
            return NotFound("No stores found.");
        }
        return Ok(stores);
    }

    // POST: /Store
    [HttpPost]
    public async Task<ActionResult<Store>> CreateStore([FromBody] Store store)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newStore = await _storeService.CreateStore(store);
        return CreatedAtAction(nameof(GetStoreById), new { id = newStore.Id }, newStore);
    }

    // PUT: /Store/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Store>> UpdateStore(int id, [FromBody] Store store)
    {
        if (id != store.Id)
        {
            return BadRequest("Store ID mismatch.");
        }

        var updatedStore = await _storeService.UpdateStore(id, store);
        if (updatedStore == null)
        {
            return NotFound($"Store with ID: {id} not found.");
        }

        return Ok(updatedStore);
    }

    // DELETE: /Store/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(int id)
    {
        var success = await _storeService.DeleteStore(id);
        if (!success)
        {
            return NotFound($"Store with ID: {id} not found.");
        }

        return NoContent();  
    }

    // GET: /Store/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Store>> GetStoreById(int id)
    {
        var store = await _storeService.GetStoreById(id);
        if (store == null)
        {
            return NotFound($"Store with ID: {id} not found.");
        }
        return store;
    }
}
