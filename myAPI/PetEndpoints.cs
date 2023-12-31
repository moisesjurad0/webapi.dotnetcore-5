﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace myAPI;

public static class PetEndpoints
{
    public static void MapPetEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Pet").WithTags(nameof(Pet));

        group.MapGet("/", [Authorize] async ([FromServices] YourDbContext db) =>
        {
            return await db.Pets.ToListAsync();
        })
        .WithName("GetAllPets")
        .WithOpenApi();

        group.MapGet("/take/{quantity}", [Authorize] async ([FromServices] YourDbContext db, int quantity) =>
        {
            return await db.Pets.Take(quantity).ToListAsync();
        })
        .WithName("GetTakedPets")
        .WithOpenApi();

        group.MapGet("/{id}", [Authorize] async Task<Results<Ok<Pet>, NotFound>> (int id, [FromServices] YourDbContext db) =>
        {
            return await db.Pets.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Pet model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPetById")
        .WithOpenApi();

        group.MapPut("/{id}", [Authorize] async Task<Results<Ok, NotFound>> (int id, Pet pet, [FromServices] YourDbContext db) =>
        {
            var affected = await db.Pets
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, pet.Id)
                  .SetProperty(m => m.Name, pet.Name)
                  .SetProperty(m => m.Paws, pet.Paws)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePet")
        .WithOpenApi();

        group.MapPost("/", [Authorize] async (Pet pet, [FromServices] YourDbContext db) =>
        {
            db.Pets.Add(pet);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Pet/{pet.Id}", pet);
        })
        .WithName("CreatePet")
        .WithOpenApi();

        group.MapDelete("/{id}", [Authorize] async Task<Results<Ok, NotFound>> (int id, [FromServices] YourDbContext db) =>
        {
            var affected = await db.Pets
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePet")
        .WithOpenApi();
    }
}
