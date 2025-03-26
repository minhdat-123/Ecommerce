using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSearchConfigSynonyms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"-- Insert script for SearchConfig table with technology-related synonyms
-- Format: Each Value contains one synonym mapping per line

INSERT INTO SearchConfigs (Name, Type, Value, IsActive, CreatedDate)
VALUES ('Technology Synonyms', 1, 'smartphone , phone, mobile phone
computer, pc
laptop, ultrabook
iphone 15, apple phone 15
tablet, pad
headphones, earphones
wireless, cordless
smartwatch, wearable
television, tv
monitor, display
keyboard, typing device
mouse, pointing device
printer, printing device
router, networking device
speaker, audio device
camera, digital camera
gaming, video games
charger, power adapter
bluetooth, wireless connectivity
usb, universal serial bus
wifi, wireless network
', 1, GETDATE());

INSERT INTO SearchConfigs (Name, Type, Value, IsActive, CreatedDate)
VALUES ('Electronics Synonyms', 1, 'earbuds, in-ear headphones
smartphone case, phone cover
memory card, storage card
power bank, portable charger
external drive, portable storage
graphics card, gpu, video card
processor, cpu
motherboard, mainboard
ram, memory
ssd, solid state drive
hdd, hard disk drive
', 1, GETDATE());

INSERT INTO SearchConfigs (Name, Type, Value, IsActive, CreatedDate)
VALUES ('Appliance Synonyms', 1, 'refrigerator, fridge
microwave, microwave oven
washing machine, washer
dryer, clothes dryer
dishwasher, dish cleaner
vacuum cleaner, vacuum
air conditioner, ac
fan, cooling fan
blender, food processor
toaster, bread toaster
', 1, GETDATE());");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM SearchConfigs WHERE Name IN ('Technology Synonyms', 'Electronics Synonyms', 'Appliance Synonyms');");
        }
    }
}
