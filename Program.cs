using CarBuilder.Models;
using CarBuilder.Models.DTOs;

List <PaintColor> paintColors = new List<PaintColor> ()
{
    new PaintColor() { Id = 1, Price = 150, Color = "Firebrick Red"},
    new PaintColor() { Id = 2, Price = 200, Color = "Midnight Blue"},
    new PaintColor() { Id = 3, Price = 125, Color = "Silver"},
    new PaintColor() { Id = 4, Price = 175, Color = "Spring Green"}
};

List <Interior> interiors = new List<Interior> ()
{
    new Interior() { Id = 1, Price = 100, Material = "Beige Fabric"},
    new Interior() { Id = 2, Price = 150, Material = "Charcoal Fabric"},
    new Interior() { Id = 3, Price = 175, Material = "White Leather"},
    new Interior() { Id = 4, Price = 200, Material = "Black Leather"}
};

List <Technology> technologies = new List<Technology> ()
{
    new Technology() { Id = 1, Price = 75, Package = "Basic Package"},
    new Technology() { Id = 2, Price = 100, Package = "Navigation Package"},
    new Technology() { Id = 3, Price = 125, Package = "Visibility Package"},
    new Technology() { Id = 4, Price = 150, Package = "Ultra Package"}
};

List <Wheels> wheels = new List<Wheels> ()
{
    new Wheels() { Id = 1, Price = 100, Style = "17-inch Pair Radial"},
    new Wheels() { Id = 2, Price = 150, Style = "17-inch Pair Radial Black"},
    new Wheels() { Id = 3, Price = 175, Style = "18-inch Pair Spoke Silver"},
    new Wheels() { Id = 4, Price = 200, Style = "18-inch Pair Spoke Black"}
};

List<Order> orders = new List<Order> ()
{
    new Order() { Id = 1, PaintId = 2, InteriorId = 3, TechnologyId = 3, WheelId = 4},
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
}

app.UseHttpsRedirection();

app.MapGet("/PaintColors", () =>
{
    return paintColors.Select(pc => new PaintColor
    {
        Id = pc.Id,
        Price = pc.Price,
        Color = pc.Color
    });
});

app.MapGet("/interiors", () =>
{
    return interiors.Select(i => new Interior
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

app.MapGet("/technologies", () =>
{
    return technologies.Select(t => new Technology
    {
        Id = t.Id,
        Price = t.Price,
        Package = t.Package
    });
});

app.MapGet("/wheels", () => 
{
    return wheels.Select(w => new Wheels
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

app.MapGet("/orders", (int? paintId) =>
{
    foreach (Order order in orders)
    {
        order.Wheel = wheels.First(w => w.Id == order.WheelId);
        order.Technology = technologies.First(w => w.Id == order.TechnologyId);
        order.PaintColor = paintColors.First(w => w.Id == order.PaintId);
        order.Interior = interiors.First(w => w.Id == order.InteriorId);
    }

    List<Order> filteredOrders = orders.Where(o => !o.IsFullFilled).ToList();

    // Now, check for the paintId property to see if we should filter by that as well
    if (paintId != null)
    {
        filteredOrders = filteredOrders.Where(order => order.PaintId == paintId).ToList();
    }

    return filteredOrders.Select(o => new OrderDTO
    {
        Id = o.Id,
        Timestamp = o.Timestamp,
        TechnologyId = o.TechnologyId,
        Technology = new TechnologyDTO
        {
            Id = o.Technology.Id,
            Package = o.Technology.Package,
            Price = o.Technology.Price
        },
        WheelId = o.WheelId,
        Wheel = new WheelsDTO
        {
            Id = o.Wheel.Id,
            Style = o.Wheel.Style,
            Price = o.Wheel.Price
        },
        InteriorId = o.InteriorId,
        Interior = new InteriorDTO
        {
            Id = o.Interior.Id,
            Material = o.Interior.Material,
            Price = o.Interior.Price
        },
        PaintId = o.PaintId,
        PaintColor = new PaintColorDTO
        {
            Id = o.PaintColor.Id,
            Color = o.PaintColor.Color,
            Price = o.PaintColor.Price
        },
    }).ToList();
});

app.MapGet("/orders/{id}", (int id) => 
{
    Order order = orders.FirstOrDefault(o => o.Id == id);
    if(order == null)
    {
        return Results.NotFound();
    }

    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    return Results.Ok(new Order
    {
        Id = order.Id,
        PaintId = order.PaintId,
        PaintColor = new PaintColor
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        },
        InteriorId = order.InteriorId,
        Interior = new Interior
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        },
        TechnologyId = order.TechnologyId,
        Technology = new Technology
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        
        WheelId = order.WheelId,
        Wheel = new Wheels
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        Timestamp = order.Timestamp
    });
});

app.MapPost("/orders", ( Order order) => 
{
    
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    
    order.Timestamp = DateTime.Now;

    order.Id = orders.Max(o => o.Id) + 1;
    orders.Add(order);

    return Results.Created($"/orders/{order.Id}", new Order 
    {
        Id = order.Id,
        PaintId = order.PaintId,
        PaintColor = new PaintColor
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        },
        InteriorId = order.InteriorId,
        Interior = new Interior
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        },
        TechnologyId = order.TechnologyId,
        Technology = new Technology
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        
        WheelId = order.WheelId,
        Wheel = new Wheels
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        Timestamp = order.Timestamp
        
    });
});

app.MapPost("/orders/{Id}/fulfill", (int Id) => 
{
    Order orderToFullFill = orders.FirstOrDefault(order => order.Id == Id);

    

    orderToFullFill.IsFullFilled = true;


    return Results.Ok();
});


app.Run();


