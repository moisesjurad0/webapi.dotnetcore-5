using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Mvc;

namespace myAPI;

public static class PetEndpoints
{
    public static void MapPetEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Pet").WithTags(nameof(Pet));

        group.MapGet("/", async ([FromServices] myContext db) =>
        {
            return await db.Pets.ToListAsync();
        })
        .WithName("GetAllPets")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Pet>, NotFound>> (int id, [FromServices] myContext db) =>
        {
            return await db.Pets.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Pet model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPetById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Pet pet, [FromServices] myContext db) =>
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

        group.MapPost("/", async (Pet pet, [FromServices] myContext db) =>
        {
            db.Pets.Add(pet);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Pet/{pet.Id}",pet);
        })
        .WithName("CreatePet")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, [FromServices] myContext db) =>
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
