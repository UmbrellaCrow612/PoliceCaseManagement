Role and claim / permission  hybrid model idea:

// Example 1: Role-Based Authorization
public class RoleBasedExample
{
    // Role-based controller
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin,ProductManager")] // Using roles
        public IActionResult GetProducts()
        {
            return Ok("Products list");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only admin role can create
        public IActionResult CreateProduct()
        {
            return Ok("Product created");
        }
    }

    // Adding user to roles
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public async Task SetupAdminUser(ApplicationUser user)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
            // User now has ALL admin permissions automatically
        }
    }
}

// Example 2: Permission-Based Authorization
public class PermissionBasedExample
{
    // Permission-based controller
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = Permissions.Products.View)]
        public IActionResult GetProducts()
        {
            return Ok("Products list");
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Products.Create)]
        public IActionResult CreateProduct()
        {
            return Ok("Product created");
        }
    }

    // Granular permission assignment
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public async Task SetupUserPermissions(ApplicationUser user)
        {
            // Can assign specific permissions without giving full admin access
            var permissions = new[]
            {
                Permissions.Products.View,
                Permissions.Orders.View,
                Permissions.Products.Create
            };
            
            await AssignPermissions(user, permissions);
        }
    }
}

// Hybrid Approach: Combining Roles and Permissions
public class HybridAuthorizationExample
{
    public class PermissionsByRole
    {
        public static Dictionary<string, string[]> RolePermissions = new()
        {
            { 
                "Admin", new[] 
                {
                    Permissions.Products.View,
                    Permissions.Products.Create,
                    Permissions.Products.Edit,
                    Permissions.Products.Delete,
                    Permissions.Orders.View,
                    Permissions.Orders.Create
                }
            },
            { 
                "ProductManager", new[]
                {
                    Permissions.Products.View,
                    Permissions.Products.Edit,
                    Permissions.Orders.View
                }
            }
        };
    }

    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public async Task SetupUserWithRoleAndExtraPermissions(ApplicationUser user)
        {
            // 1. Assign base role
            await _userManager.AddToRoleAsync(user, "ProductManager");

            // 2. Add extra permissions for this specific user
            var extraPermissions = new[]
            {
                Permissions.Orders.Create // Give extra permission not in their role
            };
            
            await AssignPermissions(user, extraPermissions);
        }
    }
}