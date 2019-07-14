using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
   public  class BLLColor
    {
        static Object key = new object();
        private static volatile BLLColor _Instance;
        public static BLLColor Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLColor();

                return _Instance;
            }
        }

        private BLLColor() { }

        public List<ColorModel> Gets()
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    return db.P_Color.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).Select(x => new ColorModel() { Id = x.Id, Name = x.Name, Note = x.Note }).ToList();
                }
                catch (Exception)
                {
                }
                return new List<ColorModel>();
            }
        }

        public List<ModelSelectItem> GetSelects()
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    return db.P_Color.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Name }).ToList();
                }
                catch (Exception)
                {
                }
                return new List<ModelSelectItem>();
            }
        }

        public ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var obj = db.P_Color.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Xóa mã màu thành công.", Title = "Thông Báo" });
                }
                else
                {
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin mã màu . Xóa mã màu thất bại.", Title = "Lỗi CSDL" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin mã màu . Xóa mã màu thất bại.", Title = "Lỗi Exception" });
            }
            return result;
        }

        public ResponseBase InsertOrUpdate(P_Color objModel)
        {
            var rs = new ResponseBase();
            using (var db = new PMSEntities())
            {
                try
                {
                    if (CheckName(objModel.Id, objModel.Name.Trim(), db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { msg = "Tên mã màu đã tồn tại. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                    }
                    else
                    {
                        if (objModel.Id == 0)
                        {
                            db.P_Color.Add(objModel);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            var oldObj = db.P_Color.FirstOrDefault(x => !x.IsDeleted && x.Id == objModel.Id);
                            if (oldObj != null)
                            {
                                oldObj.Name = objModel.Name;
                                oldObj.Note = objModel.Note;
                                rs.IsSuccess = true;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.Messages.Add(new Message() { msg = "mã màu đang thao tác không tồn tại hoặc đã bị xóa. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }

        private P_Color CheckName(int Id, string name, PMSEntities db)
        {
            return db.P_Color.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().Equals(name));
        }
    }
}
