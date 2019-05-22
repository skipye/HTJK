using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelProject
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string StrContent { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? MessageCount { get; set; }
        public string UserName { get; set; }
        public Guid? UserId { get; set; }
        public string CheckCode { get; set; }
        public int? ReplyCount { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string ReplyName { get; set; }
        public Guid ReplyUserId { get; set; }
        public bool? IsCheck { get; set; }
        public Guid TitleId { get; set; }
        public IEnumerable<ReplyModel> ReplyList { get; set; }
    }
    public class UserCurrentModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
    }
    public class ReplyModel
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string StrContent { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UserName { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsRead { get; set; }
    }
}
