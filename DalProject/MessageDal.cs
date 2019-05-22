using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DataBase;

namespace DataProject
{
    public class MessageDal
    {
        public void AddMessage(MessageModel Models)
        {
            using (var db = new HTJKEntities())
            {
                var Tabels = new MemberMessage();
                Tabels.Id = Guid.NewGuid();
                Tabels.StrContent = Models.StrContent;
                Tabels.CreateTime = DateTime.Now;
                Tabels.IsRead = false;
                Tabels.UserName = Models.UserName;
                Tabels.State = true;
                Tabels.MemberId = Models.UserId;
                db.MemberMessage.Add(Tabels);

                db.SaveChanges();
            }
        }
        public void AddReplyMessage(MessageModel Models)
        {
            using (var db = new HTJKEntities())
            {
                var Tabels = new ReplyMemMSG();
                Tabels.Id = Guid.NewGuid();
                Tabels.MemMsgId = Models.TitleId;
                Tabels.StrContent = Models.StrContent;
                Tabels.CreateTime = DateTime.Now;
                Tabels.IsRead = false;
                Tabels.UserName = Models.UserName;
                Tabels.MemberId = Models.UserId;
                Tabels.ReplyMemberId = Models.ReplyUserId;
                Tabels.State = true;
                db.ReplyMemMSG.Add(Tabels);

                db.SaveChanges();
            }
        }
        public List<MessageModel> GetMessagelist(int PageIndex, int PageSize, Guid? UserId)
        {
            using (var db = new HTJKEntities())
            {
                var list = (from p in db.MemberMessage.Where(k => k.State == true)
                            where UserId != Guid.Empty && UserId != null ? p.MemberId == UserId : true
                            orderby p.CreateTime descending
                            select new MessageModel
                            {
                                Id = p.Id,
                                StrContent = p.StrContent,
                                CreateTime = p.CreateTime,
                                UserName = p.UserName,
                                UserId = p.MemberId,
                                ReplyCount = p.ReplyMemMSG.Where(m => m.State == true).Count(),
                                ReplyDate = p.ReplyMemMSG.Where(m => m.State == true).OrderByDescending(k => k.CreateTime).Select(k => k.CreateTime).FirstOrDefault(),
                                ReplyName = p.ReplyMemMSG.Where(m => m.State == true).OrderByDescending(k => k.CreateTime).Select(k => k.UserName).FirstOrDefault(),
                                ReplyList = p.ReplyMemMSG.Where(k => k.State == true).Select(k => new ReplyModel { Id = k.Id, UserName = k.UserName, StrContent = k.StrContent, CreateTime = k.CreateTime })
                            }).ToList();
                return list.Skip(PageIndex * PageSize).Take(PageSize).ToList();
            }
        }
       
        public int? GetMessagelistConut(Guid? UserId)
        {
            using (var db = new HTJKEntities())
            {
                return db.MemberMessage.Where(k => k.State == true && k.MemberId == UserId).Count();
            }
        }
        //更新会员信息阅读状态
        public void UpdateMemberMessageState(Guid UserId)
        {
            using (var db = new HTJKEntities())
            {
                var list = db.MemberMessage.Where(k => k.State == true && k.MemberId == UserId).ToList();
                foreach (var item in list)
                {
                    item.IsRead = true;
                }
                db.SaveChanges();
            }
        }
    }
}
