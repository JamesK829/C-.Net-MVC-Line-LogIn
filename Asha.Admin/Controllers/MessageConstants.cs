using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AshaAdmin.Controllers
{
    public class MessageConstants
    {
        /*** 失敗 ***/
        public static String FAILURE = "發生錯誤，請稍後再試，謝謝。";

        /*** 新增成功 ***/
        public static String INSERT_SUCCESS = "新增資料成功，謝謝。";

        /*** 修改成功 ***/
        public static String UPDATE_SUCCESS = "異動資料成功，謝謝。";

        /*** 刪除成功 ***/
        public static String DELETE_SUCCESS = "刪除資料成功，謝謝。";

        /*** 新增失敗 ***/
        public static String INSERT_FAILURE = "新增時發生錯誤，請稍後再試，謝謝。";

        /*** 修改失敗 ***/
        public static String UPDATE_FAILURE = "修改時發生錯誤，請稍後再試，謝謝。";

        /*** 刪除失敗 ***/
        public static String DELET_FAILURE = "刪除時發生錯誤，請稍後再試，謝謝。";

        /*** 查無資料 ***/
        public static String DATA_NOT_FOUND = "無此資料,請重新操作，謝謝。";

        public static String INCORRECT = "此帳號無本後台操作權限，請洽後台管理員!";

        /*** 上傳的檔案類型錯誤 ***/
        public static String FILE_NOT_MAPPING = "上傳的檔案類型錯誤。";
        /*** 上傳的檔案類型錯誤 ***/
        public static String FILE_OVER_LIMIT = "上傳的檔案大小超過限制。";

        /*** 匯入成功 ***/
        public static String IMPORT_SUCCESS = "匯入成功。";
    }
}