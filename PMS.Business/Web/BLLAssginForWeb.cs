﻿using GPRO.Ultilities;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Business.Web.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business.Web
{
    public class BLLAssginForWeb
    {
        static Object key = new object();
        private static volatile BLLAssginForWeb _Instance;
        public static BLLAssginForWeb Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLAssginForWeb();

                return _Instance;
            }
        }

        private BLLAssginForWeb() { }

        private bool CheckOrderIndex(int stt, int LineId, int orderIndex, PMSEntities db)
        {
            var check = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT != stt && x.MaChuyen == LineId && x.STTThucHien == orderIndex);
            if (check != null)
                return true;
            return false;
        }

        public ResponseBase InsertOrUpdate(Chuyen_SanPham model)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT != model.STT && x.MaChuyen == model.MaChuyen && x.MaSanPham == model.MaSanPham && !x.IsFinish);
                    if (csp == null)
                    {

                        if (!CheckOrderIndex(model.STT, model.MaChuyen, model.STTThucHien, db))
                        {
                            if (model.STT == 0)
                            {
                                #region add
                                csp = new Chuyen_SanPham();
                                Parse.CopyObject(model, ref csp);
                                var monthDetail = new P_MonthlyProductionPlans();
                                monthDetail.Chuyen_SanPham = csp;
                                monthDetail.Month = DateTime.Now.Month;
                                monthDetail.Year = DateTime.Now.Year;
                                monthDetail.ProductionPlans = csp.SanLuongKeHoach;
                                csp.P_MonthlyProductionPlans = new List<P_MonthlyProductionPlans>();
                                csp.P_MonthlyProductionPlans.Add(monthDetail);
                                csp.TimeAdd = DateTime.Now;
                                db.Chuyen_SanPham.Add(csp);
                                db.SaveChanges();
                                result.IsSuccess = true;
                                result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });

                                #endregion
                            }
                            else
                            {
                                csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == model.STT);
                                if (csp != null)
                                {
                                    var mDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => !x.IsDeleted && x.STT_C_SP == model.STT && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                                    if (mDetail != null)
                                    {
                                        if (csp.SanLuongKeHoach > model.SanLuongKeHoach)
                                            mDetail.ProductionPlans -= (csp.SanLuongKeHoach - model.SanLuongKeHoach);
                                        else
                                            mDetail.ProductionPlans += (model.SanLuongKeHoach - csp.SanLuongKeHoach);
                                    }

                                    csp.STTThucHien = model.STTThucHien;
                                    csp.MaSanPham = model.MaSanPham;
                                    csp.UpdatedDate = model.UpdatedDate;
                                    csp.SanLuongKeHoach = model.SanLuongKeHoach;
                                    csp.IsFinish = csp.SanLuongKeHoach > csp.LuyKeTH ? false : true;
                                    csp.IsFinishBTPThoatChuyen = model.IsFinishBTPThoatChuyen;

                                    // update lai nang suat san suat va dinh muc ngay hien tai
                                    var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                                    var tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == csp.STT && x.Ngay == ngay);
                                    var nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == csp.STT && x.Ngay == ngay);
                                    if (tp != null && nx != null)
                                    {
                                        var tgLVTrongNgay = (int)BLLShift.GetTotalWorkingHourOfLine(csp.MaChuyen).TotalSeconds;
                                        tp.NangXuatLaoDong = (float)Math.Round((tgLVTrongNgay / csp.SanPham.ProductionTime), 2);

                                        nx.DinhMucNgay = (float)Math.Round((tp.NangXuatLaoDong * tp.LaoDongChuyen), 1);
                                        nx.NhipDoSanXuat = (float)Math.Round((csp.SanPham.ProductionTime / tp.LaoDongChuyen), 1);
                                        nx.TimeLastChange = DateTime.Now.TimeOfDay;
                                    }
                                    db.SaveChanges();
                                    //BLLProductivity.ResetNormsDayAndBTPInLine(2, obj.MaChuyen, false);
                                    result.IsSuccess = true;
                                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });
                                }
                                else
                                {
                                    result.IsSuccess = false;
                                    result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "không tìm thấy Phân Công.\n" });
                                }
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Messages.Add(new Message() { Title = "Thông Báo Trùng số thứ tự", msg = "Số thứ tự thực hiện  này được chọn. Vui lòng chọn số thứ tự thực hiện khác." });
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Sản phẩm này đã được phân công và đang sản xuất.\n" });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Lưu Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }

        public List<ModelSelectItem> GetNSNgayCuaChuyen(int LineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var now = DateTime.Now.ToString("d/M/yyyy");
                    if (LineId != 0)
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.HideForever &&
                            !x.SanPham.IsDelete && x.MaChuyen == LineId)
                            .OrderBy(x => x.STTThucHien)
                            .Select(x => new ModelSelectItem()
                            {
                                Value = x.STT,
                                Data = x.STT,
                                Name = x.SanPham.TenSanPham
                            }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NhapSLModel> GetDayInfo(int LineId, string date)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    if (LineId != 0)
                        return db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.HideForever &&
                            !x.Chuyen_SanPham.SanPham.IsDelete && x.Chuyen_SanPham.MaChuyen == LineId && x.Ngay == date)
                            .OrderBy(x => x.Chuyen_SanPham.STTThucHien)
                            .Select(x => new NhapSLModel()
                            {
                                cspId = x.STTCHuyen_SanPham,
                                ERR = (x.SanLuongLoi - x.SanLuongLoiGiam),
                                TC = (x.BTPThoatChuyenNgay - x.BTPThoatChuyenNgayGiam),
                                DinhMuc = x.DinhMucNgay,
                                LK_TC = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                                KCS = (x.ThucHienNgay - x.ThucHienNgayGiam),
                                LK_KCS = x.Chuyen_SanPham.LuyKeTH,
                                ColorName = x.Chuyen_SanPham.P_Color.Name,
                                ProName = x.Chuyen_SanPham.SanPham.TenSanPham,
                                SizeName = x.Chuyen_SanPham.P_Size.Name
                            }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase NhapSL(int cspId, int productType, bool isIncrease, int quantity, string ngay, int equipCode, int errorId, int clusterId, int appId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var ns = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == ngay && cspId == x.STTCHuyen_SanPham);
                    if (ns != null)
                    {
                        var tdn = new PMS.Data.TheoDoiNgay();
                        tdn.STT = 0;
                        tdn.MaChuyen = ns.Chuyen_SanPham.MaChuyen;
                        tdn.MaSanPham = ns.Chuyen_SanPham.MaSanPham;
                        tdn.CumId = clusterId;
                        tdn.EquipmentId = equipCode;
                        tdn.STTChuyenSanPham = cspId;
                        tdn.ThanhPham = quantity;
                        tdn.CommandTypeId = isIncrease ? (int)eCommandRecive.ProductIncrease : (int)eCommandRecive.ProductReduce;

                        switch (productType)
                        {
                            case 0: tdn.ProductOutputTypeId = (int)eProductOutputType.TC; break;
                            case 1: tdn.ProductOutputTypeId = (int)eProductOutputType.KCS; break;
                            case 2: tdn.ProductOutputTypeId = (int)eProductOutputType.BTP; break;
                            case 3: tdn.ProductOutputTypeId = (int)eProductOutputType.BTP_HC; break;
                            case 4:
                                tdn.ProductOutputTypeId = (int)eProductOutputType.Error;
                                tdn.ErrorId = errorId;
                                break;
                        }
                        var cf = db.Config_App.FirstOrDefault(x => x.AppId == appId && x.Config.Name.Trim().ToUpper().Equals(eAppConfigName.TypeOfCheckFinishProduction.Trim().ToUpper()));
                        var rs = BLLDayInfo.InsertOrUpdate(tdn, appId, false, (cf != null ? cf.Value.ToUpper().Trim().Split(',').ToList() : new List<string>() { "BTP", "KCS" }));
                        if (!rs.IsSuccess)
                            rs.DataSendKeyPad = rs.Messages[0].msg;
                        return rs;
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<ErrorModel> GetErrors(int cspId, string ngay)
        {
            using (var db = new PMSEntities())
            {
                var errors = db.Errors.Where(x => !x.IsDeleted && !x.GroupError.IsDeleted).Select(x => new ErrorModel()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    GroupName = x.GroupError.Name,
                    PhaseName = x.P_Phase.Name,
                    Quantity = 0
                }).OrderBy(x => x.Code).ToList();
                if (errors.Count > 0)
                {
                    var errsInDay = db.TheoDoiNgays.Where(x => ngay == x.Date && x.ErrorId.HasValue && x.STTChuyenSanPham == cspId).ToList();
                    if (errsInDay.Count > 0)
                    {
                        foreach (var item in errors)
                        {
                            int tang = errsInDay.Where(x => x.ErrorId == item.Code && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                            tang = tang - errsInDay.Where(x => x.ErrorId == item.Code && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                            item.Quantity = tang;
                        }
                    }
                }
                return errors;
            }
        }
    }
}
