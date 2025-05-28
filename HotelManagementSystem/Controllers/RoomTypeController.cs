using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Data.Models.RoomType;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;

namespace HotelManagementSystem.Controllers
{
    //[Route("api/[controller]")]
    [Route("admin/roomtypes")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }


        [HttpGet]
        public async Task<ActionResult<BasedResponseModel>> GetRoomTypes()
        {
            try
            {
                var result = await _roomTypeService.GetRoomTypes();
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result); 
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BasedResponseModel>> GetRoomTypeById(Guid id)
        {
            try
            {
                var result = await _roomTypeService.GetRoomTypeById(id);
                return !result.IsError? APIHelper.GenerateSuccessResponse(result.Result): APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BasedResponseModel>> CreateRoomType(CreateRoomTypeRequestModel model)
        {
            try
            {
                var result = await _roomTypeService.CreateRoomType(model);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }          
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<BasedResponseModel>> UpdateRoomType (Guid id, UpdateRoomTypeRequestModel requestModel)
        {
            try
            {
                var result = await _roomTypeService.UpdateRoomType(id, requestModel);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch(Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BasedResponseModel>> DeleteRoomType (Guid id)
        {
            try
            {
                var result = await _roomTypeService.DeleteRoomType(id);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }


        [HttpPost("Post")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                // Check file size
                if (uploadedFile.Length > 1024 * 1024 * 10) // 10 MB
                {
                    return BadRequest("File is too large");
                }

                // Define the path where you want to save the file
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploadPath); // Create the directory if it doesn't exist

                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                // Convert file to Base64 string
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                string base64String = Convert.ToBase64String(fileBytes);

                // Return the base64 string (or you could store it or send it somewhere)
                return Ok(new { FileName = fileName, Base64 = base64String });
            }
            return BadRequest("No file uploaded");
        }


        public class TestRequest
        {
            public IFormFile file { get; set; } 
        }
    }
}
