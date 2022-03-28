using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using TicketCore.ViewModels.Ordering;

namespace TicketCore.Data.MenuMasters.Command
{
    public class OrderingCommand : IOrderingCommand
    {
        private readonly IConfiguration _configuration;
        public OrderingCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                foreach (var menuCategory in menuCategorylist)
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuCategoryId", menuCategory.MenuCategoryId);
                    param.Add("@RoleId", menuCategory.RoleId);
                    param.Add("@SortingOrder", menuCategory.SortingOrder);
                    connection.Execute("Usp_UpdateMenuCategoryOrder", param, transaction, 0, CommandType.StoredProcedure);
                }
                sqlDataAccessManager.Commit();
            }
            catch (System.Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }
        }

        public void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);

            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                foreach (var menu in menuStoringOrder)
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuId", menu.MenuId);
                    param.Add("@RoleId", menu.RoleId);
                    param.Add("@SortingOrder", menu.SortingOrder);
                    connection.Execute("Usp_UpdateMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
                }
            }
            catch (System.Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }
        }

        public void UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder)
        {
            using SqlConnectionManager sqlDataAccessManager = new SqlConnectionManager(_configuration);
            try
            {
                var (connection, transaction) = sqlDataAccessManager.StartTransaction();
                foreach (var submenu in submenuStoringOrder)
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuId", submenu.MenuId);
                    param.Add("@RoleId", submenu.RoleId);
                    param.Add("@SortingOrder", submenu.SortingOrder);
                    param.Add("@SubMenuId", submenu.SubMenuId);
                    connection.Execute("Usp_UpdateSubMenuOrder", param, transaction, 0, CommandType.StoredProcedure);
                }
            }
            catch (System.Exception)
            {
                sqlDataAccessManager.Rollback();
                throw;
            }
        }
    }
}