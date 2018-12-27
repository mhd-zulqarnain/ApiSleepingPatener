﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiSleepingPatener
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SleepingPartnermanagementTestingEntities : DbContext
    {
        public SleepingPartnermanagementTestingEntities()
            : base("name=SleepingPartnermanagementTestingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<AdvertiserPackage> AdvertiserPackages { get; set; }
        public virtual DbSet<AdvertiserRegistration> AdvertiserRegistrations { get; set; }
        public virtual DbSet<BonusSetting> BonusSettings { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EWalletPayoutConfig> EWalletPayoutConfigs { get; set; }
        public virtual DbSet<EWalletTransaction> EWalletTransactions { get; set; }
        public virtual DbSet<EWalletWithdrawalFund> EWalletWithdrawalFunds { get; set; }
        public virtual DbSet<HelpRequest> HelpRequests { get; set; }
        public virtual DbSet<NewsTicker> NewsTickers { get; set; }
        public virtual DbSet<NewUserRegistration> NewUserRegistrations { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<ReceiveAdminMessage> ReceiveAdminMessages { get; set; }
        public virtual DbSet<ReceiveUserMessage> ReceiveUserMessages { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<SentAdminMessage> SentAdminMessages { get; set; }
        public virtual DbSet<SentUserMessage> SentUserMessages { get; set; }
        public virtual DbSet<SignUpUser> SignUpUsers { get; set; }
        public virtual DbSet<TableLevel> TableLevels { get; set; }
        public virtual DbSet<TrainingVideo> TrainingVideos { get; set; }
        public virtual DbSet<UserCommission> UserCommissions { get; set; }
        public virtual DbSet<UserCommissionBalance> UserCommissionBalances { get; set; }
        public virtual DbSet<UserGenealogyTable> UserGenealogyTables { get; set; }
        public virtual DbSet<UserGenealogyTableLeft> UserGenealogyTableLefts { get; set; }
        public virtual DbSet<UserGenealogyTableRight> UserGenealogyTableRights { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfoes { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<UserPackage> UserPackages { get; set; }
        public virtual DbSet<UserPosition> UserPositions { get; set; }
        public virtual DbSet<UserReward> UserRewards { get; set; }
        public virtual DbSet<UserTableLevel> UserTableLevels { get; set; }
        public virtual DbSet<VideoPackCatTbl> VideoPackCatTbls { get; set; }
        public virtual DbSet<VideoPackTbl> VideoPackTbls { get; set; }
    
        public virtual ObjectResult<GetParentChildsOuterRightSP_Result> GetParentChildsOuterRightSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetParentChildsOuterRightSP_Result>("GetParentChildsOuterRightSP", userIdParameter);
        }
    
        public virtual ObjectResult<GetParentChildsRightSP_Result> GetParentChildsRightSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetParentChildsRightSP_Result>("GetParentChildsRightSP", userIdParameter);
        }
    
        public virtual ObjectResult<GetParentChildsSP_Result> GetParentChildsSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetParentChildsSP_Result>("GetParentChildsSP", userIdParameter);
        }
    
        public virtual int GetUserLoginInfo(string userName, string password)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetUserLoginInfo", userNameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<GetUserParentsSP_Result> GetUserParentsSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUserParentsSP_Result>("GetUserParentsSP", userIdParameter);
        }
    
        public virtual int GetUserTree(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetUserTree", userIdParameter);
        }
    
        public virtual int GetUserTreeLeft(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetUserTreeLeft", userIdParameter);
        }
    
        public virtual int GetUserTreeRight(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetUserTreeRight", userIdParameter);
        }
    
        public virtual int insert_tree_node(string tree_name, Nullable<int> parent_ID, Nullable<int> userId, Nullable<int> downlineMemberId, string userPosition)
        {
            var tree_nameParameter = tree_name != null ?
                new ObjectParameter("tree_name", tree_name) :
                new ObjectParameter("tree_name", typeof(string));
    
            var parent_IDParameter = parent_ID.HasValue ?
                new ObjectParameter("parent_ID", parent_ID) :
                new ObjectParameter("parent_ID", typeof(int));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var downlineMemberIdParameter = downlineMemberId.HasValue ?
                new ObjectParameter("DownlineMemberId", downlineMemberId) :
                new ObjectParameter("DownlineMemberId", typeof(int));
    
            var userPositionParameter = userPosition != null ?
                new ObjectParameter("UserPosition", userPosition) :
                new ObjectParameter("UserPosition", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insert_tree_node", tree_nameParameter, parent_IDParameter, userIdParameter, downlineMemberIdParameter, userPositionParameter);
        }
    
        public virtual int move_node_down(Nullable<int> tree_id)
        {
            var tree_idParameter = tree_id.HasValue ?
                new ObjectParameter("tree_id", tree_id) :
                new ObjectParameter("tree_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("move_node_down", tree_idParameter);
        }
    
        public virtual int move_node_up(Nullable<int> tree_id)
        {
            var tree_idParameter = tree_id.HasValue ?
                new ObjectParameter("tree_id", tree_id) :
                new ObjectParameter("tree_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("move_node_up", tree_idParameter);
        }
    
        public virtual int remove_node(Nullable<int> tree_id)
        {
            var tree_idParameter = tree_id.HasValue ?
                new ObjectParameter("tree_id", tree_id) :
                new ObjectParameter("tree_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("remove_node", tree_idParameter);
        }
    
        public virtual int view_human_tree()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("view_human_tree");
        }
    
        public virtual int view_tree()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("view_tree");
        }
    
        public virtual ObjectResult<GetParentChildsLeftSP_Result> GetParentChildsLeftSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetParentChildsLeftSP_Result>("GetParentChildsLeftSP", userIdParameter);
        }
    
        public virtual ObjectResult<GetParentChildsOuterLeftSP_Result> GetParentChildsOuterLeftSP(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetParentChildsOuterLeftSP_Result>("GetParentChildsOuterLeftSP", userIdParameter);
        }
    }
}