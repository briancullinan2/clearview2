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
        public DbSet<DataLayer.Entities.Setting> Settings { get; set; }

        public TranslationContext(string connection) : base()
        {
            _currentString = _connection = connection;

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
        public static TranslationContext Get(string name) => _contexts.GetOrAdd(name, (key) => new TranslationContext(key));

        public TranslationContext this[string name]
        {
            get
            {
                // MULTI-LINE LOGIC HERE
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                _currentString = name;
                return _contexts.GetOrAdd(name, (key) => new TranslationContext(key));
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
            modelBuilder.Entity<Calibration>().ToTable(EntityMetadata.Calibration.TableName);
            modelBuilder.Entity<Capture>().ToTable(EntityMetadata.Capture.TableName);
            modelBuilder.Entity<Device>().ToTable(EntityMetadata.Device.TableName);
            modelBuilder.Entity<DeviceCalibrationSetting>().ToTable(EntityMetadata.DeviceCalibrationSetting.TableName);
            modelBuilder.Entity<DeviceSetting>().ToTable(EntityMetadata.DeviceSetting.TableName);
            modelBuilder.Entity<FingerSet>().ToTable(EntityMetadata.FingerSet.TableName);
            modelBuilder.Entity<Image>().ToTable(EntityMetadata.Image.TableName);
            modelBuilder.Entity<ImageAlignment>().ToTable(EntityMetadata.ImageAlignment.TableName);
            modelBuilder.Entity<ImageCalibration>().ToTable(EntityMetadata.ImageCalibration.TableName);
            modelBuilder.Entity<ImageCapture>().ToTable(EntityMetadata.ImageCapture.TableName);
            modelBuilder.Entity<ImageSector>().ToTable(EntityMetadata.ImageSector.TableName);
            modelBuilder.Entity<Message>().ToTable(EntityMetadata.Message.TableName);
            modelBuilder.Entity<Patient>().ToTable(EntityMetadata.Patient.TableName);
            modelBuilder.Entity<Permission>().ToTable(EntityMetadata.Permission.TableName);
            modelBuilder.Entity<Role>().ToTable(EntityMetadata.Role.TableName);
            modelBuilder.Entity<Setting>().ToTable(EntityMetadata.Setting.TableName);
            modelBuilder.Entity<User>().ToTable(EntityMetadata.User.TableName);

            /*
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasMaxLength(50); // This is the "Gold Standard" for EF
            */

            /*
            modelBuilder.Entity<MedicalMessage>()
                .Property(e => e.VoltageSetting)
                .HasConversion<int>();
            */

        }

        public static TranslationContext Current => Get(_currentString);
        private static string _currentString = "Data Source=:memory:";
    }

    public partial class EntityMetadata
    {
        public static EntityMetadata<Entities.Calibration> Calibration => new EntityMetadata<Entities.Calibration>();
        public static EntityMetadata<Entities.Capture> Capture => new EntityMetadata<Entities.Capture>();
        public static EntityMetadata<Entities.Device> Device => new EntityMetadata<Entities.Device>();
        public static EntityMetadata<Entities.DeviceCalibrationSetting> DeviceCalibrationSetting => new EntityMetadata<Entities.DeviceCalibrationSetting>();
        public static EntityMetadata<Entities.DeviceSetting> DeviceSetting => new EntityMetadata<Entities.DeviceSetting>();
        public static EntityMetadata<Entities.FingerSet> FingerSet => new EntityMetadata<Entities.FingerSet>();
        public static EntityMetadata<Entities.Image> Image => new EntityMetadata<Entities.Image>();
        public static EntityMetadata<Entities.ImageCapture> ImageCapture => new EntityMetadata<Entities.ImageCapture>();
        public static EntityMetadata<Entities.ImageAlignment> ImageAlignment => new EntityMetadata<Entities.ImageAlignment>();
        public static EntityMetadata<Entities.ImageCalibration> ImageCalibration => new EntityMetadata<Entities.ImageCalibration>();
        public static EntityMetadata<Entities.ImageSector> ImageSector => new EntityMetadata<Entities.ImageSector>();
        public static EntityMetadata<Entities.Message> Message => new EntityMetadata<Entities.Message>();
        public static EntityMetadata<Entities.Patient> Patient => new EntityMetadata<Entities.Patient>();
        public static EntityMetadata<Entities.Permission> Permission => new EntityMetadata<Entities.Permission>();
        public static EntityMetadata<Entities.Role> Role => new EntityMetadata<Entities.Role>();
        public static EntityMetadata<Entities.Setting> Setting => new EntityMetadata<Entities.Setting>();
        public static EntityMetadata<Entities.User> User => new EntityMetadata<Entities.User>();

    }

}
