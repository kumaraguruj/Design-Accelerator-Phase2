using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DA.DomainModel;
using DA.BusinessLayer;

namespace DesignAccelerator.Models.ViewModel
{
    public class ProductsViewModel
    {
        #region Public Properties
        public int ProductID { get; set; }

        [Display(Name = "Enter Requirement reference")]
        [Required(ErrorMessage = "Requirement reference required")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Special Characters are not allowed in this field")]
        public string ReqReference { get; set; }

        public int LobID { get; set; }
        public string LobDesc { get; set; }
        public int BuzProdID { get; set; }
        public string BuzProdDesc { get; set; }

        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int daid { get; set; }
        public string daName { get; set; }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ProductName { get; set; }

        public IList<tbl_LOB> lstLOB { get; set; }
        public IList<tbl_BuzProd> lstBuzProd { get; set; }
        public IList<tbl_Products> lstProd { get; set; }

        public int roleId { get; set; }
        public string RoleName { get; set; }

        public bool AddPermmission = false;
        public bool EdiPermission = false;
        public bool DeletePermission = false;

        #endregion

        public ProductsViewModel GetProducts(int? designAccelaratorID)
        {
            try
            {
                ProductsViewModel prodviewmodel = new ProductsViewModel();
                ProductsManager prodManager = new ProductsManager();

                prodviewmodel.lstLOB = prodManager.GetLOBs(designAccelaratorID);
                prodviewmodel.lstBuzProd = prodManager.GetBuzProds(designAccelaratorID);
                prodviewmodel.lstProd = prodManager.GetAllProducts(designAccelaratorID);

                return prodviewmodel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddProduct(ProductsViewModel prodviewmodel)
        {
            try
            {
                tbl_Products tblprod = new tbl_Products();

                tblprod.ReqReference = prodviewmodel.ReqReference;
                tblprod.daid = prodviewmodel.daid;
                tblprod.BuzProdID = prodviewmodel.BuzProdID;
                tblprod.LobID = prodviewmodel.LobID;
                tblprod.EntityState = DA.DomainModel.EntityState.Added;

                ProductsManager prodManager = new ProductsManager();
                prodManager.AddProduct(tblprod);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductsViewModel FindProd(int? ProdID)
        {
            try
            {
                ProductsViewModel prodsvm = new ProductsViewModel();
                ProductsManager prodManager = new ProductsManager();


                var prods = prodManager.FindProduct(ProdID);
                var daid1 = prods.daid;
                prodsvm.daid = daid1;

                prodsvm.lstLOB = prodManager.GetLOBs(daid1);
                prodsvm.lstBuzProd = prodManager.GetBuzProds(daid1);

                prodsvm.ProductID = prods.ProductID;
                prodsvm.ReqReference = prods.ReqReference;
                prodsvm.BuzProdID = prods.BuzProdID;
                //to get name instead of id
                prodsvm.BuzProdDesc = prodsvm.lstBuzProd.Where(e => e.BuzProdID.Equals(prods.BuzProdID)).First().BuzProdDesc;
                prodsvm.LobID = prods.LobID;
                prodsvm.LobDesc = prodsvm.lstLOB.Where(e => e.LobID.Equals(prods.LobID)).First().LobDesc;
                prodsvm.daid = prods.daid;

                //prodsvm.lstProd = (IList<tbl_Products>) prods;

                return prodsvm;
            }
            catch (Exception)
            {
                throw;
            }
        }
            

        public bool DeleteProd(ProductsViewModel prodviewmodel)
        {
            try
            {
                tbl_Products tblprod = new tbl_Products();

                tblprod.ProductID = prodviewmodel.ProductID;
                tblprod.EntityState = DA.DomainModel.EntityState.Deleted;

                ProductsManager prodManager = new ProductsManager();
                prodManager.DeleteProduct(tblprod);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProd(ProductsViewModel prodviewmodel)
        {
            try
            {
                tbl_Products tblprod = new tbl_Products();

                tblprod.ProductID = prodviewmodel.ProductID;
                tblprod.ReqReference = prodviewmodel.ReqReference;
                tblprod.daid = prodviewmodel.daid;
                tblprod.BuzProdID = prodviewmodel.BuzProdID;
                tblprod.LobID = prodviewmodel.LobID;
                tblprod.EntityState = DA.DomainModel.EntityState.Modified;

                ProductsManager prodManager = new ProductsManager();
                prodManager.UpdateProduct(tblprod);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicate(ProductsViewModel prodviewmodel)
        {
            try
            {
                ProductsManager prodManager = new ProductsManager();

                var product = prodManager.FindProductReqRef(prodviewmodel.ReqReference, prodviewmodel.LobID, prodviewmodel.BuzProdID, prodviewmodel.daid);

                if (product != null && product.ProductID != prodviewmodel.ProductID && product.ReqReference.ToUpper() == prodviewmodel.ReqReference.ToUpper()
                    && product.LobID == prodviewmodel.LobID && product.BuzProdID == prodviewmodel.BuzProdID)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_LOB> GetLOBS(ProductsViewModel prodViewModel)
        {
            try
            {
                LOBManager lobManager = new LOBManager();
                prodViewModel.lstLOB = lobManager.GetLOBDetails(daid);
                return prodViewModel.lstLOB;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<tbl_BuzProd> GetBusinessProds(ProductsViewModel prodViewModel)
        {
            try
            {
                BuzProdManager BPManager = new BuzProdManager();
                prodViewModel.lstBuzProd = BPManager.GetBusinessProducts(daid);
                return prodViewModel.lstBuzProd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetScreenAccessRights(string screenName)
        {
            try
            {
                tbl_UserData currentloggedinuserdata = (tbl_UserData)HttpContext.Current.Session["CurrentLoggedInUserDetails"];
                roleId = currentloggedinuserdata.RoleID;

                RoleManager roleManager = new RoleManager();
                var userrolepermissions = roleManager.GetUserViewAccessPermissions(screenName, roleId);

                foreach (var item in userrolepermissions)
                {
                    if (item.ActionType == "Add")
                        AddPermmission = true;
                    else if (item.ActionType == "Edit")
                        EdiPermission = true;
                    else if (item.ActionType == "Delete")
                        DeletePermission = true;

                    RoleName = item.RoleName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}