namespace CarCare_Companion.Api.Controllers.Admin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Admin.Users;

using static Common.GlobalConstants;

[Route("[controller]")]
public class UsersController : BaseAdminController
{
    private readonly IIdentityService identityService;
    private readonly IUserService userService;
    private readonly ILogger<UsersController> logger;

    public UsersController(IIdentityService identityService, IUserService userService, ILogger<UsersController> logger)
    {
        this.identityService = identityService;
        this.userService = userService;
        this.logger = logger;
    }


    /// <summary>
    /// Retrieves a user with all of his details
    /// </summary>
    /// <param name="id">the user identifier</param>
    /// <returns>A model containing the user details</returns>
    [HttpGet]
    [Route("ApplicationUsers/{id}")]
    [ProducesResponseType(200, Type = typeof(UserDetailsResponseModel))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserAdministrator = await identityService.IsUserInRoleAsync(userId, AdministratorRoleName);

            if (!isUserAdministrator)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            bool incomingUserExist = await identityService.DoesUserExistByIdAsync(userId);

            if (!incomingUserExist)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.InvalidData,

                });
            }

            UserDetailsResponseModel user = await userService.GetUserDetailsByIdAsync(id);

    
            return StatusCode(200, user);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.InvalidData,

            });
        }
    }


    /// <summary>
    /// Retrieves all application users
    /// </summary>
    /// <returns>Collection of the application users</returns>
    [HttpGet]
    [Route("ApplicationUsers")]
    [ProducesResponseType(200, Type = typeof(ICollection<UserInformationResponseModel>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ApplicationUsers()
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserAdministrator = await identityService.IsUserInRoleAsync(userId, AdministratorRoleName);

            if (!isUserAdministrator)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            ICollection<UserInformationResponseModel> allApplicationUsers = await userService.GetAllUsersAsync();

            return StatusCode(200, allApplicationUsers);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.InvalidData,

            });
        }
    }

    /// <summary>
    /// Adds an user to the administrator role
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <returns>A boolen based on the adding result/returns>
    [HttpPatch]
    [Route("ApplicationUsers/AddAdmin/{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AddAdminByUserId([FromRoute] string id)
    {
        try
        {
            string? userId = this.User.GetId();

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(403, new ProblemDetails
                {
                    Title = StatusResponses.InvalidUser
                });
            }

            bool isUserAdministrator = await identityService.IsUserInRoleAsync(userId, AdministratorRoleName);

            if (!isUserAdministrator)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            bool incomingUserExist = await identityService.DoesUserExistByIdAsync(id);

            if (!incomingUserExist)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.InvalidData,

                });
            }

            bool isIncomingUserAdmin = await identityService.IsUserInRoleAsync(id, AdministratorRoleName);

            if (isIncomingUserAdmin)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.AlreadyAdmin,
                });
            }

            bool isAddedToAdminRole = await identityService.AddAdminAsync(id);


            if (!isAddedToAdminRole)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,
                });
            }

            return StatusCode(200, isAddedToAdminRole);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.InvalidData,

            });
        }
    }

    /// <summary>
    /// Removes an user from administrator role
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <returns>A boolean based on the removing result/returns>
    [HttpPatch]
    [Route("ApplicationUsers/RemoveAdmin/{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RemoveAdminByUserId([FromRoute] string id)
    {
        try
        {
            string userId = User.GetId()!;

            bool isUserAdministrator = await identityService.IsUserInRoleAsync(userId, AdministratorRoleName);

            if (!isUserAdministrator)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            bool incomingUserExist = await identityService.DoesUserExistByIdAsync(id);

            if (!incomingUserExist)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.InvalidData,

                });
            }

            bool isIncomingUserAdmin = await identityService.IsUserInRoleAsync(id, AdministratorRoleName);

            if (!isIncomingUserAdmin)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            bool isRemovedFromAdminRole = await identityService.RemoveAdminAsync(id);


            if (!isRemovedFromAdminRole)
            {
                return StatusCode(400, new ProblemDetails()
                {
                    Detail = StatusResponses.BadRequest,

                });
            }

            return StatusCode(200, isRemovedFromAdminRole);

        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.BadRequest,

            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return StatusCode(400, new ProblemDetails()
            {
                Detail = StatusResponses.InvalidData,

            });
        }
    }

}
