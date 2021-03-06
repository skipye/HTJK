﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DataBase;
using System.Web.Mvc;

namespace DalProject
{
    public class UserDal
    {
        //操作日志
        public void AddWorkLogs(WorkLogsModel tables)
        {
            using (var db = new HTJKEntities())
            {
                var WorkLogs = new WorkLogs();
                WorkLogs.UserId = tables.UserId;
                WorkLogs.UserName = tables.UserName;
                WorkLogs.MSG = tables.MSG;
                WorkLogs.MSGStatus = tables.MSGStatus;
                WorkLogs.CreateTime = DateTime.Now;
                db.WorkLogs.Add(WorkLogs);
                db.SaveChanges();
            }
        }
        public List<UsersModel> GetPageList(SUsersModel SModel)
        {

            using (var db = new HTJKEntities())
            {
                var List = (from p in db.A_User.Where(k => k.State == true)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.Name.Contains(SModel.Name) : true
                            orderby p.CreateTime descending
                            select new UsersModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Telphone = p.Telphone,
                                LoginTimes = p.LoginTimes,
                                LastTimes = p.LastTimes,
                            }).ToList();
                return List;
            }
        }
        //用户登录
        public LoginModel IsLogin(LoginModel models)
        {
            using (var db = new HTJKEntities())
            {
                var Tables = (from p in db.A_User.Where(k => k.Password == models.PassWord && k.State == true)
                              where !string.IsNullOrEmpty(models.UserName) ? p.Name == models.UserName : true
                              where !string.IsNullOrEmpty(models.Telephone) ? p.Telphone == models.Telephone : true
                              select p
                             ).FirstOrDefault();
                LoginModel ReturnModel = new LoginModel();
                if (Tables != null)
                {
                    ReturnModel.UserName = Tables.Name;
                    ReturnModel.IsLogin = true;
                    ReturnModel.UserId = Tables.Id;

                    Tables.LoginTimes = Tables.LoginTimes + 1;
                    Tables.LastTimes = DateTime.Now;
                    db.SaveChanges();
                }
                else { ReturnModel.IsLogin = false; }
               
                return ReturnModel;
            }
        }

        public void AddOrUpdate(UsersModel Models)
        {
            using (var db = new HTJKEntities())
            {
                if (Models.Id > 0)
                {
                    var table = db.A_User.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.Telphone = Models.Telphone;
                    table.Name = Models.Name;
                    table.Password = Models.Password;

                }
                else
                {
                    A_User table = new A_User();
                    table.Telphone = Models.Telphone;
                    table.Name = Models.Name;
                    table.Password = Models.Password;
                    table.LastTimes = DateTime.Now;
                    table.LoginTimes = 1;
                    table.CreateTime = DateTime.Now;
                    db.A_User.Add(table);
                }
                db.SaveChanges();
            }
        }
        public UsersModel GetDetailById(int Id)
        {
            using (var db = new HTJKEntities())
            {
                var tables = (from p in db.A_User.Where(k => k.Id == Id)
                              select new UsersModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Telphone = p.Telphone,
                                  Password = p.Password,
                                  Password2 = p.Password,
                              }).SingleOrDefault();
                return tables;
            }
        }
        public void Delete(string ListId)
        {
            using (var db = new HTJKEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.A_User.Where(k => k.Id == Id).SingleOrDefault();
                        tables.State = false;
                    }
                }
                db.SaveChanges();
            }
        }
        public List<SelectListItem> GetUserDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择用户", Value = "" });
            using (var db = new HTJKEntities())
            {
                List<A_User> model = db.A_User.Where(b => b.State == true).OrderBy(k => k.CreateTime).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.Name, Value = item.Id.ToString(), Selected = pId.HasValue && item.Id.Equals(pId) });
                }
            }
            return items;
        }
    }
}
