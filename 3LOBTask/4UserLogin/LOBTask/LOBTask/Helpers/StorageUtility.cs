using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LOBTask.Helpers
{
    /// <summary>
    /// Storage 相關的 API
    /// http://www.nudoq.org/#!/Packages/PCLStorage/PCLStorage/FileSystem
    /// </summary>
    public class StorageUtility
    {
        /// <summary>
        /// 將所指定的字串寫入到指定目錄的檔案內
        /// </summary>
        /// <param name="folderName">目錄名稱</param>
        /// <param name="filename">檔案名稱</param>
        /// <param name="content">所要寫入的文字內容</param> 
        /// <returns></returns>
        public static async Task WriteToDataFileAsync(string folderPath, string folderName, string filename, string content)
        {
            // 宣告根目錄的物件變數
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            #region 檢查傳入的參數內容是否有問題
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }
            #endregion

            // 在 C# 內，要進行資源(網路、檔案、作業系統資源等)存取的時候，請要將這些程式碼 try ... catch
            try
            {
                #region 建立與取得指定路徑內的資料夾
                string[] fooPaths = folderName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                IFolder folder = rootFolder;
                foreach (var item in fooPaths)
                {
                    folder = await folder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                }
                #endregion

                IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                await file.WriteAllTextAsync(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 從指定目錄的檔案內將文字內容讀出
        /// </summary>
        /// <param name="folderName">目錄名稱</param>
        /// <param name="filename">檔案名稱</param>
        /// <returns>文字內容</returns>
        public static async Task<string> ReadFromDataFileAsync(string folderPath, string folderName, string filename)
        {
            string content = "";

            // 宣告根目錄的物件變數
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            #region 檢查傳入的參數內容是否有問題
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }
            #endregion

            // 在 C# 內，要進行資源(網路、檔案、作業系統資源等)存取的時候，請要將這些程式碼 try ... catch
            try
            {
                #region 建立與取得指定路徑內的資料夾
                string[] fooPaths = folderName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                IFolder folder = rootFolder;
                foreach (var item in fooPaths)
                {
                    folder = await folder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                }
                #endregion

                var fooallf = await folder.GetFilesAsync();
                var fooExist = fooallf.FirstOrDefault(x => x.Name == filename);
                if (fooExist != null)
                {
                    IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

                    content = await file.ReadAllTextAsync();
                }
                else
                {
                    content = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }

            return content.Trim();
        }
    }
}
