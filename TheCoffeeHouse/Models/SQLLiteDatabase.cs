using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheCoffeeHouse.Models
{
    class SQLLiteDatabase
    {
        public string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool Create()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                connection.CreateTable<TinTuc>();
                connection.CreateTable<KhachHang>();
                connection.CreateTable<SP_SIZE>();
                connection.CreateTable<LoaiKhachHang>();
                connection.CreateTable<LoaiSanPham>();
                connection.CreateTable<CuaHangChiTiet>();
                connection.CreateTable<CuaHangYeuThich>();
                connection.CreateTable<SanPham>();
                connection.CreateTable<DotKhuyenMai>();
                connection.CreateTable<Cart>();
                connection.CreateTable<DiaChiDonHang>();
                connection.CreateTable<DiaChiNguoiDung>();
                
                //Code mới
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
        }
        public KhachHang CheckUserLogIn()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<KhachHang> kh = connection.Query<KhachHang>("SELECT * FROM KhachHang");
                if (kh.Count!=0)
                {
                    return kh[0];
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public bool AddListSanPham(List<SanPham> sanPhams)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM SanPham");
                for (int i = 0; i < sanPhams.Count; i++)
                {
                    connection.Insert(sanPhams[i]);
                }
            }
            catch (Exception)
            {

                return false;
                throw;
            }
            return true;
        }
        public bool AddListLoaiSanPham(List<LoaiSanPham> loaiSanPhams)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM LoaiSanPham");
                for (int i = 0; i < loaiSanPhams.Count; i++)
                {
                    connection.Insert(loaiSanPhams[i]);
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool AddListDotKhuyenMai(List<DotKhuyenMai> dotKhuyenMais)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM DotKhuyenMai");
                for (int i = 0; i < dotKhuyenMais.Count; i++)
                {
                    connection.Insert(dotKhuyenMais[i]);
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool AddListSize(List<SP_SIZE> sP_SIZEs)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM SP_SIZE");
                for (int i = 0; i < sP_SIZEs.Count; i++)
                {
                    connection.Insert(sP_SIZEs[i]);
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool InsertKhachHang(KhachHang kh)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Insert(kh);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public KhachHang GetKhachHang()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<KhachHang> khachHangs = connection.Query<KhachHang>("SELECT * FROM KhachHang");
                if (khachHangs.Count == 0)
                {
                    return null;
                }
                return khachHangs[0];
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public List<LoaiKhachHang> GetLoaiKhachHangs()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<LoaiKhachHang> loaiKhachHangs = connection.Query<LoaiKhachHang>("SELECT * FROM LoaiKhachHang");
                if (loaiKhachHangs.Count == 0)
                {
                    return null;
                }
                return loaiKhachHangs;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public int GetChietKhauKhachHang()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<KhachHang> khachHang = connection.Query<KhachHang>("SELECT * FROM KhachHang");
                string LoaiKh = khachHang[0].LoaiKH;
                List<LoaiKhachHang> loaiKhachHang = connection.Query<LoaiKhachHang>("SELECT * FROM LoaiKhachHang WHERE LoaiKH='" + LoaiKh + "'");
                return loaiKhachHang[0].ChietKhauHoaDon;
            }
            catch (Exception)
            {

                return 0;
                throw;
            }
        }
        public List<SanPham> GetSanPham()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<SanPham> loaiSanPhams = connection.Query<SanPham>("SELECT * FROM SanPham");
                if (loaiSanPhams.Count == 0)
                {
                    return null;
                }
                return loaiSanPhams;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public List<LoaiSanPham> GetLoaiSanPham()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<LoaiSanPham> loaiSanPhams = connection.Query<LoaiSanPham>("SELECT * FROM LoaiSanPham");
                if (loaiSanPhams.Count == 0)
                {
                    return null;
                }
                return loaiSanPhams;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public List<DotKhuyenMai> GetDotKhuyenMai()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DotKhuyenMai> loaiDotKhuyenMais = connection.Query<DotKhuyenMai>("SELECT * FROM DotKhuyenMai");
                if (loaiDotKhuyenMais.Count == 0)
                {
                    return null;
                }
                return loaiDotKhuyenMais;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public LoaiKhachHang GetLoaiKhachHangByMa(string LoaiKH)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<LoaiKhachHang> loaiKhachHangs = connection.Query<LoaiKhachHang>("SELECT * FROM LoaiKhachHang WHERE LoaiKH='" + LoaiKH + "'");
                if (loaiKhachHangs.Count == 0)
                {
                    return null;
                }
                return loaiKhachHangs[0];
            }
            catch (Exception e)
            {

                return null;
                throw e;
            }
        }
        public bool AddListLoaiKhachHang(List<LoaiKhachHang> loaiKhachHangs)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM LoaiKhachHang");
                for (int i = 0; i < loaiKhachHangs.Count; i++)
                {
                    connection.Insert(loaiKhachHangs[i]);
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool AddListCuaHang(List<CuaHangChiTiet> lstCuahang)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM CuaHangChiTiet");
                for (int i = 0; i < lstCuahang.Count; i++)
                {
                    connection.Insert(lstCuahang[i]);
                }
            }
            catch (SQLiteException)
            {
                return false;
                throw;
            }
            return true;
        }
        public List<CuaHangChiTiet> GetCuaHangChiTiets()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<CuaHangChiTiet> lstCuaHang = connection.Query<CuaHangChiTiet>("SELECT * FROM CuaHangChiTiet");
                if (lstCuaHang.Count != 0)
                {
                    return lstCuaHang;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public List<CuaHangChiTiet> GetCuaHangYeuThichs()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<CuaHangChiTiet> lstCuaHang = connection.Query<CuaHangChiTiet>("SELECT * FROM CuaHangYeuThich");
                if (lstCuaHang.Count != 0)
                {
                    return lstCuaHang;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public bool AddCuaHangYeuThich(CuaHangYeuThich cuaHang)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Insert(cuaHang);
            }
            catch (SQLiteException)
            {
                return false;
                throw;
            }
            return true;
        }
        public int RemoveCuaHangYeuThich(CuaHangYeuThich cuaHang)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                return connection.Execute("DELETE FROM CuaHangYeuThich WHERE MaCH = " + cuaHang.MaCH.ToString());
            }
            catch (SQLiteException)
            {
                return 0;
                throw;
            }
        }
        //New code
        public List<SP_SIZE> GetSizeSPbyMaSP(int MaSP)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<SP_SIZE> listSize = connection.Query<SP_SIZE>("SELECT * FROM SP_SIZE WHERE MaSP=" + MaSP);
                if (listSize.Count == 0)
                {
                    return null;
                }
                return listSize;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public List<Cart> GetCart()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<Cart> listCart = connection.Query<Cart>("SELECT * FROM Cart");
                if (listCart.Count == 0)
                {
                    return null;
                }
                return listCart;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public bool AddToCart(Cart cart)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<Cart> listCart = connection.Query<Cart>("SELECT * FROM Cart WHERE MASP="+cart.MaSP+" AND Size='"+cart.Size+"'");
                if (listCart.Count==0)
                {
                    connection.Insert(cart);
                }
                else
                {
                    listCart[0].SoLuong += cart.SoLuong;
                    connection.Update(listCart[0]);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool DeleteCart()
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM Cart");
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool DeleteACart(Cart c)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Delete(c);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool ChooseCuaHang(DiaChiDonHang dc)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DiaChiDonHang> DCDH = connection.Query<DiaChiDonHang>("SELECT * FROM DiaChiDonHang WHERE PhuongThucNhanHang=1");
                if (DCDH.Count == 0)
                {
                    connection.Insert(dc);
                }
                else
                {
                    connection.Update(dc);
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public DiaChiDonHang GetCuaHangDonHang()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DiaChiDonHang> DCDH = connection.Query<DiaChiDonHang>("SELECT * FROM DiaChiDonHang WHERE PhuongThucNhanHang=1");
                if (DCDH.Count == 0)
                {
                    return null;
                }
                else
                {
                    return DCDH[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<DiaChiNguoiDung> GetDiaChiNguoiDung()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DiaChiNguoiDung> DCND = connection.Query<DiaChiNguoiDung>("SELECT * FROM DiaChiNguoiDung");
                if (DCND.Count == 0)
                {
                    return null;
                }
                else
                {
                    return DCND;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool DeleteDiaChiDonHang()
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM DiaChiDonHang");
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public bool InsertDiaChiNguoiDung(DiaChiNguoiDung DCND)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                connection.Insert(DCND);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool ChooseDiaChiNguoiDung(DiaChiDonHang dc)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DiaChiDonHang> DCDH = connection.Query<DiaChiDonHang>("SELECT * FROM DiaChiDonHang WHERE PhuongThucNhanHang=2");
                if (DCDH.Count == 0)
                {
                    connection.Insert(dc);
                }
                else
                {
                    connection.Update(dc);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DiaChiDonHang GetDiaChiNguoiDungGiaoHang()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<DiaChiDonHang> DCDH = connection.Query<DiaChiDonHang>("SELECT * FROM DiaChiDonHang WHERE PhuongThucNhanHang=2");
                if (DCDH.Count == 0)
                {
                    return null;
                }
                else
                {
                    return DCDH[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Code lan 2
        //Code mới nhất
        public int CapNhatThongTinKhachHang(string makh, string hoten, string sdt, string ngaysinh, int gioitinh)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            KhachHang kh = new KhachHang
            {
                MaKH = makh,
                HoTen = hoten,
                SDT = sdt,
                NgaySinh = Convert.ToDateTime(ngaysinh),
                GioiTinh = Convert.ToByte(gioitinh)
            };
            try
            {
                return connection.Update(kh);
            }
            catch (SQLiteException ex)
            {
                return 0;
                throw;
            }
        }
        public bool AddListTinTuc(List<TinTuc> lstTinTuc)
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM TinTuc");
                for (int i = 0; i < lstTinTuc.Count; i++)
                {
                    connection.Insert(lstTinTuc[i]);
                }
            }
            catch (SQLiteException)
            {
                return false;
                throw;
            }
            return true;
        }
        public List<TinTuc> GetTinTucs()
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<TinTuc> lstTinTuc = connection.Query<TinTuc>("SELECT * FROM TinTuc");
                if (lstTinTuc.Count != 0)
                {
                    return lstTinTuc;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public CuaHangChiTiet GetCuaHangByMa(int MaCH )
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<CuaHangChiTiet> lstCuaHang = connection.Query<CuaHangChiTiet>("SELECT * FROM CuaHangChiTiet WHERE MaCH="+MaCH);
                if (lstCuaHang.Count != 0)
                {
                    return lstCuaHang[0];
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public SanPham GetSanPhamByMa(int MaSP)
        {
            try
            {
                string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
                var connection = new SQLiteConnection(path);
                List<SanPham> lstCuaHang = connection.Query<SanPham>("SELECT * FROM SanPham WHERE MaSP=" + MaSP);
                if (lstCuaHang.Count != 0)
                {
                    return lstCuaHang[0];
                }
                return null;
            }
            catch (Exception)
            {

                return null;
                throw;
            }
        }
        public void DangXuat()
        {
            string path = System.IO.Path.Combine(folder, "TheCoffeeHouse.db");
            var connection = new SQLiteConnection(path);
            try
            {
                connection.Execute("DELETE FROM KhachHang");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
