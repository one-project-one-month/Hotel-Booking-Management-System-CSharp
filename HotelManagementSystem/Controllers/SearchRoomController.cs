using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.SearchRoom;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchRoomController : ControllerBase
    {
        private readonly ISearchRoomService _service;

        public SearchRoomController(ISearchRoomService service)
        {
            _service = service;
        }



        [HttpPost]

        public async Task<ActionResult<BasedResponseModel>> SearchRoom([FromBody] SearchRoomRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                #region CheckRequiredField
                // if (String.IsNullOrEmpty(model.UserId))
                // {
                //     return APIHelper.GenerateResponseForRequiredField(nameof(model.UserId), _sharedLocalizer);
                // }
                #endregion

                #region Check Format
                // if(model.UserType != EntitiesConstant.USER_TYPE.USER.GetHashCode())
                // {
                //     return BadRequest(ErrorMessageConstant.EM_UserTypeNotAcceptable); ///Not Acceptable
                // }
                #endregion

                var result = await _service.SearchRoom(model);

                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(500);
            }

        }
    }
}
