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
    
    public partial class SleepingtestEntities : DbContext
    {
        public SleepingtestEntities()
            : base("name=SleepingtestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<NewUserRegistration> NewUserRegistrations { get; set; }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<BonusSetting> BonusSettings { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EWalletPayoutConfig> EWalletPayoutConfigs { get; set; }
        public virtual DbSet<EWalletTransaction> EWalletTransactions { get; set; }
        public virtual DbSet<EWalletWithdrawalFund> EWalletWithdrawalFunds { get; set; }
        public virtual DbSet<HelpRequest> HelpRequests { get; set; }
        public virtual DbSet<NewsTicker> NewsTickers { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<ReceiveAdminMessage> ReceiveAdminMessages { get; set; }
        public virtual DbSet<ReceiveUserMessage> ReceiveUserMessages { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<SentAdminMessage> SentAdminMessages { get; set; }
        public virtual DbSet<SentUserMessage> SentUserMessages { get; set; }
        public virtual DbSet<SignUpUser> SignUpUsers { get; set; }
        public virtual DbSet<TableLevel> TableLevels { get; set; }
        public virtual DbSet<UserGenealogyTable> UserGenealogyTables { get; set; }
        public virtual DbSet<UserGenealogyTableLeft> UserGenealogyTableLefts { get; set; }
        public virtual DbSet<UserGenealogyTableRight> UserGenealogyTableRights { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfoes { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<UserPackage> UserPackages { get; set; }
        public virtual DbSet<UserPosition> UserPositions { get; set; }
        public virtual DbSet<UserTableLevel> UserTableLevels { get; set; }
    }
}
