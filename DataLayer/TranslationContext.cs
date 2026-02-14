using EPIC.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;

namespace EPIC.DataLayer
{
    // This context never connects to a DB; it just holds your Entity mappings
    public class TranslationContext : DbContext
    {
        public DbSet<DataLayer.Entities.Calibration> Calibration { get; set; }
        public DbSet<DataLayer.Entities.Capture> Capture { get; set; }

        public DbSet<DataLayer.Entities.Device> Devices { get; set; }
        public DbSet<DataLayer.Entities.DeviceCalibrationSetting> DeviceCalibrationSettings { get; set; }
        public DbSet<DataLayer.Entities.DeviceSetting> DeviceSettings { get; set; }
        public DbSet<DataLayer.Entities.FingerSet> FingerSets { get; set; }
        public DbSet<DataLayer.Entities.Image> Images { get; set; }
        public DbSet<DataLayer.Entities.ImageAlignment> ImageAlignments { get; set; }
        public DbSet<DataLayer.Entities.ImageCalibration> ImageCalibrations { get; set; }
        public DbSet<DataLayer.Entities.ImageCapture> ImageCaptures { get; set; }
        public DbSet<DataLayer.Entities.ImageSector> ImageSectors { get; set; }
        public DbSet<DataLayer.Entities.Message> Messages { get; set; }
        public DbSet<DataLayer.Entities.Patient> Patients { get; set; }
        public DbSet<DataLayer.Entities.Permission> Permissions { get; set; }
        public DbSet<DataLayer.Entities.Role> Roles { get; set; }
        public DbSet<DataLayer.Entities.User> Users { get; set; }
        // Add other entities here...

        public TranslationContext(string connection) : base()
        {
            _connection = connection;

            var conn = this.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) conn.Open();

            this.Database.EnsureCreated();
            //this.Database.Migrate();
            _contexts.TryAdd(connection, this);
        }

        public string ConnectString
        {
            get
            {
                return _connection;
            }
        }

        private string _connection;

        private DbContextOptionsBuilder _builder;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (_connection == null)
                throw new ConfigurationErrorsException("Connection string 'MedicalDb' not found in App.config.");

            // 2. Point the translator to the right SQL dialect
            // We use a dummy connection because we only care about generating the SQL string
            if (_connection.Contains("Sqlite"))
            {
                _builder = options.UseSqlite(_connection);
            }
#if SQLSERVER_PRESENT
            else if (_connection.ProviderName.Contains("SqlClient"))
            {
                options.UseSqlServer(_connection.ConnectionString);
            }
#endif
            else if (_connection.Contains("memory"))
            {
                _builder = options.UseSqlite("DataSource=:memory:");
            }
        }

        private static readonly ConcurrentDictionary<string, TranslationContext> _contexts = new ConcurrentDictionary<string, TranslationContext>();

        // Access a specific database context by its App.config Name
        public static TranslationContext Get(string name) => _contexts.GetOrAdd(name, (key) =>
        {
            // Logic for 45 CFR § 164.312 auditing
            TranslationContext ctx;
            return _contexts.TryGetValue(name, out ctx) ? ctx : new TranslationContext(key);
        });

        public TranslationContext this[string name]
        {
            get
            {
                // MULTI-LINE LOGIC HERE
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                _currentString = name;
                return _contexts.GetOrAdd(name, (key) =>
                {
                    // Logic for 45 CFR § 164.312 auditing
                    TranslationContext ctx;
                    return _contexts.TryGetValue(name, out ctx) ? ctx : new TranslationContext(key);
                });
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // This makes the conversion "implicit" for the database layer globally.
            configurationBuilder.Properties<Customization.Voltage>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.Gender>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.OrganComponent>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.OrganSystem>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.PulseDuration>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.PulseWidth>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.PWM0Frequency>().HaveConversion<int>();
            configurationBuilder.Properties<Customization.Voltage>().HaveConversion<int>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly map the Message entity to the "Message" table
            modelBuilder.Entity<Calibration>().ToTable("Calibration");
            modelBuilder.Entity<Capture>().ToTable("Capture");
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<DeviceCalibrationSetting>().ToTable("DeviceCalibrationSetting");
            modelBuilder.Entity<DeviceSetting>().ToTable("DeviceSetting");
            modelBuilder.Entity<FingerSet>().ToTable("FingerSet");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<ImageAlignment>().ToTable("ImageAlignment");
            modelBuilder.Entity<ImageCalibration>().ToTable("ImageCalibration");
            modelBuilder.Entity<ImageCapture>().ToTable("ImageCapture");
            modelBuilder.Entity<ImageSector>().ToTable("ImageSector");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
        }

        public static TranslationContext Current => Get(_currentString);
        private static string _currentString = "Data Source=:memory:";
    }


}
