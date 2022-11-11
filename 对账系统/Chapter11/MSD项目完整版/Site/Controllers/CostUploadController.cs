using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Site.Controllers
{
    [Authorize]
    public class CostUploadController : Controller
    {
        string msg = "上传成功!成本上传后，24小时可查到";

        /// <summary>
        /// 是否是日期格式
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns></returns>
        bool IsDateString(string str)
        {
            string regDate = @"^[12]\d{3}(0\d|1[0-2])([0-2]\d|3[01])$";

            Regex reg = new Regex(regDate);

            return reg.IsMatch(str);
        }

        //
        // GET: /CostUpload/
        /// <summary>
        /// 运单成本页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CostUpload()
        {
            return View();
        }
        /// <summary>
        /// 运单成本上传
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult OrderCostUpload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    if (fileName.Length < 8)
                    {
                        return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string strFile = fileName.Substring(0, 8);
                        if (!IsDateString(strFile))
                        {
                            return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/OrderCost/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    //string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                    string saveName = fileName;// +"_" + DateTime.Now.ToString("yyyyMMddHHmmss"); // 保存文件名称
                    fileData.SaveAs(filePath + saveName);
                    return Json(new { Success = true, Message = msg + "运单对账单!", FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///提单成本上传
        /// </summary>
        /// <returns></returns>
         [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult BillCostUpload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    if (fileName.Length < 8)
                    {
                        return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string strFile = fileName.Substring(0, 8);
                        if (!IsDateString(strFile))
                        {
                            return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/BillCost/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    //string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                    string saveName = fileName;// +"_" + DateTime.Now.ToString("yyyyMMddHHmmss"); // 保存文件名称
                    fileData.SaveAs(filePath + saveName);
                    return Json(new { Success = true, Message = msg + "提单对账单!", FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///身份证验证成本上传
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult IDECostUpload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    if (fileName.Length < 8)
                    {
                        return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string strFile = fileName.Substring(0, 8);
                        if (!IsDateString(strFile))
                        {
                            return Json(new { Success = false, Message = "文件格式不正确" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/IdeCost/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名          
                    string saveName = fileName;
                    fileData.SaveAs(filePath + saveName);
                    return Json(new { Success = true, Message = msg + "身份证验证对账单!", FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
