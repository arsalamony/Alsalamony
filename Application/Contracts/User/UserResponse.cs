using Application.Contracts.UserProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.User;

public record UserResponse
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<UserProductResponse> UserProducts { get; set; } = new List<UserProductResponse>();
}
