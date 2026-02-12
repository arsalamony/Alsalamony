using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Results;


namespace Application.Common.Errors;

public class ProductErrors
{
    public static readonly Error ProductAddFailed =
    new("Product.AddFailed", "Failed to add the new Product", StatusCodes.Status500InternalServerError);
}
