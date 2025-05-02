-- Insert Brands for Electronics Subcategories
INSERT INTO Brands (Name)
VALUES
('Dell'),
('HP'),
('Lenovo'),
('Apple'),
('Asus'),
('Samsung'),
('Google'),
('OnePlus'),
('Xiaomi'),
('LG'),
('Sony'),
('Panasonic'),
('Vizio'),
('Canon'),
('Nikon'),
('Fujifilm'),
('Bose'),
('JBL'),
('Sennheiser'),
('Beats'),
('Microsoft'),
('Nintendo'),
('Fitbit'),
('Garmin'),
('Huawei'),
('Logitech'),
('Belkin'),
('Anker'),
('OtterBox'),
('Spigen'),
('Whirlpool'),
('GE'),
('Bosch'),
('Epson'),
('Brother'),
('Xerox');

DECLARE @ComputersId INT = (SELECT Id FROM Categories WHERE Name = 'Computers');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ComputersId
FROM Brands
WHERE Name IN ('Dell', 'HP', 'Lenovo', 'Apple', 'Asus');

DECLARE @MobilePhonesId INT = (SELECT Id FROM Categories WHERE Name = 'Mobile Phones');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MobilePhonesId
FROM Brands
WHERE Name IN ('Apple', 'Samsung', 'Google', 'OnePlus', 'Xiaomi');

DECLARE @TelevisionsId INT = (SELECT Id FROM Categories WHERE Name = 'Televisions');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @TelevisionsId
FROM Brands
WHERE Name IN ('Samsung', 'LG', 'Sony', 'Panasonic', 'Vizio');

DECLARE @CamerasId INT = (SELECT Id FROM Categories WHERE Name = 'Cameras');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @CamerasId
FROM Brands
WHERE Name IN ('Canon', 'Nikon', 'Sony', 'Fujifilm', 'Panasonic');

DECLARE @AudioDevicesId INT = (SELECT Id FROM Categories WHERE Name = 'Audio Devices');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @AudioDevicesId
FROM Brands
WHERE Name IN ('Bose', 'Sony', 'JBL', 'Sennheiser', 'Beats');

DECLARE @GamingConsolesId INT = (SELECT Id FROM Categories WHERE Name = 'Gaming Consoles');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @GamingConsolesId
FROM Brands
WHERE Name IN ('Sony', 'Microsoft', 'Nintendo');

DECLARE @WearablesId INT = (SELECT Id FROM Categories WHERE Name = 'Wearables');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WearablesId
FROM Brands
WHERE Name IN ('Apple', 'Samsung', 'Fitbit', 'Garmin', 'Huawei');

DECLARE @ElectronicsAccessoriesId INT = (SELECT Id FROM Categories WHERE Name = 'Accessories' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Electronics'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ElectronicsAccessoriesId
FROM Brands
WHERE Name IN ('Logitech', 'Belkin', 'Anker', 'OtterBox', 'Spigen');

DECLARE @HomeAppliancesId INT = (SELECT Id FROM Categories WHERE Name = 'Home Appliances');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @HomeAppliancesId
FROM Brands
WHERE Name IN ('Whirlpool', 'GE', 'LG', 'Samsung', 'Bosch');

DECLARE @OfficeElectronicsId INT = (SELECT Id FROM Categories WHERE Name = 'Office Electronics');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OfficeElectronicsId
FROM Brands
WHERE Name IN ('Epson', 'Brother', 'Canon', 'HP', 'Xerox');

-- Insert Brands for Clothing Subcategories
INSERT INTO Brands (Name)
VALUES
('Nike'),
('Adidas'),
('Levi''s'),
('Ralph Lauren'),
('Tommy Hilfiger'),
('Zara'),
('H&M'),
('Gucci'),
('Prada'),
('Chanel'),
('Carter''s'),
('OshKosh B''gosh'),
('Gap Kids'),
('Old Navy'),
('Disney'),
('Puma'),
('Reebok'),
('New Balance'),
('Ray-Ban'),
('Oakley'),
('Fossil'),
('Michael Kors'),
('Kate Spade'),
('Under Armour'),
('Lululemon'),
('Columbia'),
('The North Face'),
('Patagonia'),
('Brooks Brothers'),
('Hugo Boss'),
('Armani'),
('Calvin Klein'),
('Versace'),
('Uniqlo'),
('Gap'),
('American Eagle'),
('Abercrombie & Fitch'),
('Hollister'),
('Canada Goose'),
('Moncler'),
('Burton'),
('Helly Hansen'),
('Marmot'),
('Victoria''s Secret'),
('Hanes'),
('Fruit of the Loom'),
('Jockey');

DECLARE @MensClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Men''s Clothing');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MensClothingId
FROM Brands
WHERE Name IN ('Nike', 'Adidas', 'Levi''s', 'Ralph Lauren', 'Tommy Hilfiger');

DECLARE @WomensClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Women''s Clothing');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WomensClothingId
FROM Brands
WHERE Name IN ('Zara', 'H&M', 'Gucci', 'Prada', 'Chanel');

DECLARE @KidsClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Kids'' Clothing');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @KidsClothingId
FROM Brands
WHERE Name IN ('Carter''s', 'OshKosh B''gosh', 'Gap Kids', 'Old Navy', 'Disney');

DECLARE @ShoesId INT = (SELECT Id FROM Categories WHERE Name = 'Shoes');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ShoesId
FROM Brands
WHERE Name IN ('Nike', 'Adidas', 'Puma', 'Reebok', 'New Balance');

DECLARE @ClothingAccessoriesId INT = (SELECT Id FROM Categories WHERE Name = 'Accessories' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Clothing'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ClothingAccessoriesId
FROM Brands
WHERE Name IN ('Ray-Ban', 'Oakley', 'Fossil', 'Michael Kors', 'Kate Spade');

DECLARE @SportswearId INT = (SELECT Id FROM Categories WHERE Name = 'Sportswear');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SportswearId
FROM Brands
WHERE Name IN ('Under Armour', 'Lululemon', 'Columbia', 'The North Face', 'Patagonia');

DECLARE @FormalWearId INT = (SELECT Id FROM Categories WHERE Name = 'Formal Wear');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FormalWearId
FROM Brands
WHERE Name IN ('Brooks Brothers', 'Hugo Boss', 'Armani', 'Calvin Klein', 'Versace');

DECLARE @CasualWearId INT = (SELECT Id FROM Categories WHERE Name = 'Casual Wear');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @CasualWearId
FROM Brands
WHERE Name IN ('Uniqlo', 'Gap', 'American Eagle', 'Abercrombie & Fitch', 'Hollister');

DECLARE @SeasonalClothingId INT = (SELECT Id FROM Categories WHERE Name = 'Seasonal Clothing');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SeasonalClothingId
FROM Brands
WHERE Name IN ('Canada Goose', 'Moncler', 'Burton', 'Helly Hansen', 'Marmot');

DECLARE @UndergarmentsId INT = (SELECT Id FROM Categories WHERE Name = 'Undergarments');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @UndergarmentsId
FROM Brands
WHERE Name IN ('Victoria''s Secret', 'Calvin Klein', 'Hanes', 'Fruit of the Loom', 'Jockey');

-- Insert Brands for Home Subcategories
INSERT INTO Brands (Name)
VALUES
('IKEA'),
('Ashley Furniture'),
('Wayfair'),
('La-Z-Boy'),
('Herman Miller'),
('KitchenAid'),
('Cuisinart'),
('Le Creuset'),
('Pyrex'),
('Tupperware'),
('Tempur-Pedic'),
('Serta'),
('Sealy'),
('Brooklinen'),
('Parachute'),
('Pottery Barn'),
('Crate & Barrel'),
('West Elm'),
('Anthropologie'),
('HomeGoods'),
('Philips'),
('Lutron'),
('Cree'),
('Feit Electric'),
('Rubbermaid'),
('Sterilite'),
('The Container Store'),
('ClosetMaid'),
('Simple Houseware'),
('DeWalt'),
('Black+Decker'),
('Craftsman'),
('Stanley'),
('Milwaukee'),
('Weber'),
('Traeger'),
('Coleman'),
('Yeti'),
('Igloo'),
('PetSmart'),
('Petco'),
('Purina'),
('Pedigree'),
('Blue Buffalo');

DECLARE @FurnitureId INT = (SELECT Id FROM Categories WHERE Name = 'Furniture');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FurnitureId
FROM Brands
WHERE Name IN ('IKEA', 'Ashley Furniture', 'Wayfair', 'La-Z-Boy', 'Herman Miller');

DECLARE @KitchenwareId INT = (SELECT Id FROM Categories WHERE Name = 'Kitchenware');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @KitchenwareId
FROM Brands
WHERE Name IN ('KitchenAid', 'Cuisinart', 'Le Creuset', 'Pyrex', 'Tupperware');

DECLARE @BeddingId INT = (SELECT Id FROM Categories WHERE Name = 'Bedding');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @BeddingId
FROM Brands
WHERE Name IN ('Tempur-Pedic', 'Serta', 'Sealy', 'Brooklinen', 'Parachute');

DECLARE @DecorId INT = (SELECT Id FROM Categories WHERE Name = 'Decor');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @DecorId
FROM Brands
WHERE Name IN ('Pottery Barn', 'Crate & Barrel', 'West Elm', 'Anthropologie', 'HomeGoods');

DECLARE @LightingId INT = (SELECT Id FROM Categories WHERE Name = 'Lighting');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @LightingId
FROM Brands
WHERE Name IN ('Philips', 'GE', 'Lutron', 'Cree', 'Feit Electric');

DECLARE @StorageId INT = (SELECT Id FROM Categories WHERE Name = 'Storage');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @StorageId
FROM Brands
WHERE Name IN ('Rubbermaid', 'Sterilite', 'The Container Store', 'ClosetMaid', 'Simple Houseware');

DECLARE @HomeAppliancesHomeId INT = (SELECT Id FROM Categories WHERE Name = 'Appliances' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Home'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @HomeAppliancesHomeId
FROM Brands
WHERE Name IN ('Whirlpool', 'GE', 'LG', 'Samsung', 'Bosch');

DECLARE @ToolsId INT = (SELECT Id FROM Categories WHERE Name = 'Tools');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ToolsId
FROM Brands
WHERE Name IN ('DeWalt', 'Black+Decker', 'Craftsman', 'Stanley', 'Milwaukee');

DECLARE @OutdoorId INT = (SELECT Id FROM Categories WHERE Name = 'Outdoor');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OutdoorId
FROM Brands
WHERE Name IN ('Weber', 'Traeger', 'Coleman', 'Yeti', 'Igloo');

DECLARE @PetsId INT = (SELECT Id FROM Categories WHERE Name = 'Pets');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PetsId
FROM Brands
WHERE Name IN ('PetSmart', 'Petco', 'Purina', 'Pedigree', 'Blue Buffalo');

-- Insert Brands for Books Subcategories
INSERT INTO Brands (Name)
VALUES
('Penguin Random House'),
('HarperCollins'),
('Simon & Schuster'),
('Hachette'),
('Macmillan'),
('O''Reilly'),
('Wiley'),
('Scholastic'),
('Disney Publishing'),
('Little, Brown Books for Young Readers'),
('Pearson'),
('McGraw-Hill'),
('Cengage'),
('Oxford University Press'),
('Marvel'),
('DC Comics'),
('Dark Horse'),
('Image Comics'),
('Viz Media'),
('Time'),
('National Geographic'),
('Vogue'),
('The New Yorker'),
('Wired'),
('Audible'),
('Libro.fm'),
('Google Play Books'),
('Amazon Kindle'),
('Barnes & Noble Nook'),
('Kobo'),
('Kaplan'),
('Princeton Review'),
('Barron''s'),
('Merriam-Webster'),
('Encyclopaedia Britannica'),
('Oxford Dictionaries');

DECLARE @FictionId INT = (SELECT Id FROM Categories WHERE Name = 'Fiction');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FictionId
FROM Brands
WHERE Name IN ('Penguin Random House', 'HarperCollins', 'Simon & Schuster', 'Hachette', 'Macmillan');

DECLARE @NonFictionId INT = (SELECT Id FROM Categories WHERE Name = 'Non-Fiction');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @NonFictionId
FROM Brands
WHERE Name IN ('Penguin Random House', 'HarperCollins', 'Simon & Schuster', 'Hachette', 'Macmillan', 'O''Reilly', 'Wiley');

DECLARE @ChildrensBooksId INT = (SELECT Id FROM Categories WHERE Name = 'Children''s Books');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ChildrensBooksId
FROM Brands
WHERE Name IN ('Scholastic', 'Disney Publishing', 'Little, Brown Books for Young Readers');

DECLARE @TextbooksId INT = (SELECT Id FROM Categories WHERE Name = 'Textbooks');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @TextbooksId
FROM Brands
WHERE Name IN ('Pearson', 'McGraw-Hill', 'Cengage', 'Wiley', 'Oxford University Press');

DECLARE @ComicsId INT = (SELECT Id FROM Categories WHERE Name = 'Comics');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ComicsId
FROM Brands
WHERE Name IN ('Marvel', 'DC Comics', 'Dark Horse', 'Image Comics', 'Viz Media');

DECLARE @MagazinesId INT = (SELECT Id FROM Categories WHERE Name = 'Magazines');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MagazinesId
FROM Brands
WHERE Name IN ('Time', 'National Geographic', 'Vogue', 'The New Yorker', 'Wired');

DECLARE @AudiobooksId INT = (SELECT Id FROM Categories WHERE Name = 'Audiobooks');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @AudiobooksId
FROM Brands
WHERE Name IN ('Audible', 'Libro.fm', 'Google Play Books');

DECLARE @EbooksId INT = (SELECT Id FROM Categories WHERE Name = 'E-books');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @EbooksId
FROM Brands
WHERE Name IN ('Amazon Kindle', 'Barnes & Noble Nook', 'Kobo');

DECLARE @EducationalBooksId INT = (SELECT Id FROM Categories WHERE Name = 'Educational');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @EducationalBooksId
FROM Brands
WHERE Name IN ('Kaplan', 'Princeton Review', 'Barron''s');

DECLARE @ReferenceId INT = (SELECT Id FROM Categories WHERE Name = 'Reference');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ReferenceId
FROM Brands
WHERE Name IN ('Merriam-Webster', 'Encyclopaedia Britannica', 'Oxford Dictionaries');

-- Insert Brands for Sports Subcategories
INSERT INTO Brands (Name)
VALUES
('Wilson'),
('Spalding'),
('Rawlings'),
('Franklin Sports'),
('Prince'),
('Ping'),
('Titleist'),
('Bowflex'),
('NordicTrack'),
('Peloton'),
('Rogue Fitness'),
('REI'),
('Osprey'),
('Speedo'),
('Arena'),
('TYR'),
('O''Neill'),
('Rossignol'),
('Salomon'),
('Atomic'),
('GoPro'),
('Gatorade'),
('Powerade'),
('Clif Bar'),
('Optimum Nutrition'),
('Mueller'),
('Cramer'),
('KT Tape');

DECLARE @TeamSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Team Sports');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @TeamSportsId
FROM Brands
WHERE Name IN ('Wilson', 'Spalding', 'Rawlings', 'Franklin Sports');

DECLARE @IndividualSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Individual Sports');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @IndividualSportsId
FROM Brands
WHERE Name IN ('Prince', 'Ping', 'Titleist');

DECLARE @FitnessEquipmentSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Fitness Equipment' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Sports'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FitnessEquipmentSportsId
FROM Brands
WHERE Name IN ('Bowflex', 'NordicTrack', 'Peloton', 'Rogue Fitness');

DECLARE @OutdoorSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Outdoor Sports');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OutdoorSportsId
FROM Brands
WHERE Name IN ('Patagonia', 'REI', 'Marmot', 'Osprey');

DECLARE @WaterSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Water Sports');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WaterSportsId
FROM Brands
WHERE Name IN ('Speedo', 'Arena', 'TYR', 'O''Neill');

DECLARE @WinterSportsId INT = (SELECT Id FROM Categories WHERE Name = 'Winter Sports');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WinterSportsId
FROM Brands
WHERE Name IN ('Burton', 'Rossignol', 'Salomon', 'Atomic');

DECLARE @SportsApparelId INT = (SELECT Id FROM Categories WHERE Name = 'Sports Apparel');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SportsApparelId
FROM Brands
WHERE Name IN ('Nike', 'Adidas', 'Under Armour', 'Puma');

DECLARE @SportsAccessoriesId INT = (SELECT Id FROM Categories WHERE Name = 'Sports Accessories');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SportsAccessoriesId
FROM Brands
WHERE Name IN ('Garmin', 'Fitbit', 'GoPro');

DECLARE @SportsNutritionId INT = (SELECT Id FROM Categories WHERE Name = 'Sports Nutrition');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SportsNutritionId
FROM Brands
WHERE Name IN ('Gatorade', 'Powerade', 'Clif Bar', 'Optimum Nutrition');

DECLARE @SportsMedicineId INT = (SELECT Id FROM Categories WHERE Name = 'Sports Medicine');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SportsMedicineId
FROM Brands
WHERE Name IN ('Mueller', 'Cramer', 'KT Tape');

-- Insert Brands for Toys Subcategories
INSERT INTO Brands (Name)
VALUES
('Hasbro'),
('Mattel'),
('Funko'),
('Bandai'),
('American Girl'),
('Madame Alexander'),
('LeapFrog'),
('Melissa & Doug'),
('VTech'),
('Ravensburger'),
('Asmodee'),
('Buffalo Games'),
('Springbok'),
('Little Tikes'),
('Step2'),
('Nerf'),
('LEGO'),
('Mega Bloks'),
('K''NEX'),
('Crayola'),
('Play-Doh'),
('Fisher-Price'),
('Gund'),
('Ty'),
('Build-A-Bear');

DECLARE @ActionFiguresId INT = (SELECT Id FROM Categories WHERE Name = 'Action Figures');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ActionFiguresId
FROM Brands
WHERE Name IN ('Hasbro', 'Mattel', 'Funko', 'Bandai');

DECLARE @DollsId INT = (SELECT Id FROM Categories WHERE Name = 'Dolls');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @DollsId
FROM Brands
WHERE Name IN ('Mattel', 'American Girl', 'Madame Alexander');

DECLARE @EducationalToysId INT = (SELECT Id FROM Categories WHERE Name = 'Educational Toys');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @EducationalToysId
FROM Brands
WHERE Name IN ('LeapFrog', 'Melissa & Doug', 'VTech');

DECLARE @BoardGamesId INT = (SELECT Id FROM Categories WHERE Name = 'Board Games');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @BoardGamesId
FROM Brands
WHERE Name IN ('Hasbro', 'Mattel', 'Ravensburger', 'Asmodee');

DECLARE @PuzzlesId INT = (SELECT Id FROM Categories WHERE Name = 'Puzzles');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PuzzlesId
FROM Brands
WHERE Name IN ('Ravensburger', 'Buffalo Games', 'Springbok');

DECLARE @OutdoorToysId INT = (SELECT Id FROM Categories WHERE Name = 'Outdoor Toys');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OutdoorToysId
FROM Brands
WHERE Name IN ('Little Tikes', 'Step2', 'Nerf');

DECLARE @BuildingToysId INT = (SELECT Id FROM Categories WHERE Name = 'Building Toys');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @BuildingToysId
FROM Brands
WHERE Name IN ('LEGO', 'Mega Bloks', 'K''NEX');

DECLARE @ArtsAndCraftsId INT = (SELECT Id FROM Categories WHERE Name = 'Arts and Crafts');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ArtsAndCraftsId
FROM Brands
WHERE Name IN ('Crayola', 'Play-Doh', 'Melissa & Doug');

DECLARE @ElectronicToysId INT = (SELECT Id FROM Categories WHERE Name = 'Electronic Toys');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ElectronicToysId
FROM Brands
WHERE Name IN ('VTech', 'LeapFrog', 'Fisher-Price');

DECLARE @StuffedAnimalsId INT = (SELECT Id FROM Categories WHERE Name = 'Stuffed Animals');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @StuffedAnimalsId
FROM Brands
WHERE Name IN ('Gund', 'Ty', 'Build-A-Bear');

-- Insert Brands for Beauty Subcategories
INSERT INTO Brands (Name)
VALUES
('Clinique'),
('Neutrogena'),
('Olay'),
('La Roche-Posay'),
('MAC'),
('Maybelline'),
('L''Oréal'),
('Estée Lauder'),
('Pantene'),
('Head & Shoulders'),
('Garnier'),
('Schwarzkopf'),
('Dior'),
('Dove'),
('Nivea'),
('Aveeno'),
('Cetaphil'),
('Gillette'),
('Old Spice'),
('Axe'),
('Dove Men+Care'),
('Bath & Body Works'),
('The Body Shop'),
('Lush'),
('Revlon'),
('Conair'),
('Philips Norelco'),
('Burt''s Bees'),
('Avalon Organics'),
('Dr. Bronner''s'),
('La Mer'),
('Sisley'),
('Tom Ford Beauty');

DECLARE @SkincareId INT = (SELECT Id FROM Categories WHERE Name = 'Skincare');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SkincareId
FROM Brands
WHERE Name IN ('Clinique', 'Neutrogena', 'Olay', 'La Roche-Posay');

DECLARE @MakeupId INT = (SELECT Id FROM Categories WHERE Name = 'Makeup');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MakeupId
FROM Brands
WHERE Name IN ('MAC', 'Maybelline', 'L''Oréal', 'Estée Lauder');

DECLARE @HaircareId INT = (SELECT Id FROM Categories WHERE Name = 'Haircare');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @HaircareId
FROM Brands
WHERE Name IN ('Pantene', 'Head & Shoulders', 'Garnier', 'Schwarzkopf');

DECLARE @FragrancesId INT = (SELECT Id FROM Categories WHERE Name = 'Fragrances');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FragrancesId
FROM Brands
WHERE Name IN ('Chanel', 'Dior', 'Gucci', 'Versace');

DECLARE @PersonalCareBeautyId INT = (SELECT Id FROM Categories WHERE Name = 'Personal Care' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Beauty'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PersonalCareBeautyId
FROM Brands
WHERE Name IN ('Dove', 'Nivea', 'Aveeno', 'Cetaphil');

DECLARE @MensGroomingId INT = (SELECT Id FROM Categories WHERE Name = 'Men''s Grooming');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MensGroomingId
FROM Brands
WHERE Name IN ('Gillette', 'Old Spice', 'Axe', 'Dove Men+Care');

DECLARE @BathAndBodyId INT = (SELECT Id FROM Categories WHERE Name = 'Bath and Body');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @BathAndBodyId
FROM Brands
WHERE Name IN ('Bath & Body Works', 'The Body Shop', 'Lush');

DECLARE @ToolsAndAccessoriesBeautyId INT = (SELECT Id FROM Categories WHERE Name = 'Tools and Accessories' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Beauty'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ToolsAndAccessoriesBeautyId
FROM Brands
WHERE Name IN ('Revlon', 'Conair', 'Philips Norelco');

DECLARE @NaturalAndOrganicId INT = (SELECT Id FROM Categories WHERE Name = 'Natural and Organic');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @NaturalAndOrganicId
FROM Brands
WHERE Name IN ('Burt''s Bees', 'Avalon Organics', 'Dr. Bronner''s');

DECLARE @LuxuryBeautyId INT = (SELECT Id FROM Categories WHERE Name = 'Luxury Beauty');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @LuxuryBeautyId
FROM Brands
WHERE Name IN ('La Mer', 'Sisley', 'Tom Ford Beauty');

-- Insert Brands for Automotive Subcategories
INSERT INTO Brands (Name)
VALUES
('Denso'),
('ACDelco'),
('MagnaFlow'),
('Harley-Davidson'),
('Yamaha'),
('Honda'),
('Kawasaki'),
('Snap-on'),
('Pioneer'),
('Kenwood'),
('Alpine'),
('Michelin'),
('Bridgestone'),
('Goodyear'),
('Pirelli'),
('WeatherTech'),
('Husky Liners'),
('Covercraft'),
('Armor All'),
('Meguiar''s'),
('Chemical Guys'),
('Mobil 1'),
('Castrol'),
('Valvoline'),
('Pennzoil'),
('Turtle Wax'),
('Griot''s Garage'),
('Cobra'),
('Viper'),
('Compustar');

DECLARE @CarPartsId INT = (SELECT Id FROM Categories WHERE Name = 'Car Parts');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @CarPartsId
FROM Brands
WHERE Name IN ('Bosch', 'Denso', 'ACDelco', 'MagnaFlow');

DECLARE @MotorcyclePartsId INT = (SELECT Id FROM Categories WHERE Name = 'Motorcycle Parts');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MotorcyclePartsId
FROM Brands
WHERE Name IN ('Harley-Davidson', 'Yamaha', 'Honda', 'Kawasaki');

DECLARE @ToolsAndEquipmentAutoId INT = (SELECT Id FROM Categories WHERE Name = 'Tools and Equipment' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Automotive'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ToolsAndEquipmentAutoId
FROM Brands
WHERE Name IN ('Snap-on', 'Craftsman', 'DeWalt', 'Milwaukee');

DECLARE @CarElectronicsId INT = (SELECT Id FROM Categories WHERE Name = 'Car Electronics');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @CarElectronicsId
FROM Brands
WHERE Name IN ('Pioneer', 'Kenwood', 'Alpine', 'Garmin');

DECLARE @TiresAndWheelsId INT = (SELECT Id FROM Categories WHERE Name = 'Tires and Wheels');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @TiresAndWheelsId
FROM Brands
WHERE Name IN ('Michelin', 'Bridgestone', 'Goodyear', 'Pirelli');

DECLARE @ExteriorAccessoriesId INT = (SELECT Id FROM Categories WHERE Name = 'Exterior Accessories');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @ExteriorAccessoriesId
FROM Brands
WHERE Name IN ('WeatherTech', 'Husky Liners', 'Covercraft');

DECLARE @InteriorAccessoriesId INT = (SELECT Id FROM Categories WHERE Name = 'Interior Accessories');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @InteriorAccessoriesId
FROM Brands
WHERE Name IN ('Armor All', 'Meguiar''s', 'Chemical Guys');

DECLARE @OilsAndFluidsId INT = (SELECT Id FROM Categories WHERE Name = 'Oils and Fluids');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OilsAndFluidsId
FROM Brands
WHERE Name IN ('Mobil 1', 'Castrol', 'Valvoline', 'Pennzoil');

DECLARE @CleaningAndDetailingId INT = (SELECT Id FROM Categories WHERE Name = 'Cleaning and Detailing');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @CleaningAndDetailingId
FROM Brands
WHERE Name IN ('Turtle Wax', 'Meguiar''s', 'Griot''s Garage');

DECLARE @SafetyAndSecurityId INT = (SELECT Id FROM Categories WHERE Name = 'Safety and Security');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SafetyAndSecurityId
FROM Brands
WHERE Name IN ('Cobra', 'Viper', 'Compustar');

-- Insert Brands for Garden Subcategories
INSERT INTO Brands (Name)
VALUES
('Proven Winners'),
('Bonnie Plants'),
('Burpee'),
('Park Seed'),
('Johnny''s Selected Seeds'),
('Baker Creek Heirloom Seeds'),
('Fiskars'),
('Corona'),
('Ames'),
('True Temper'),
('Evergreen Enterprises'),
('Alpine Corporation'),
('Achla Designs'),
('Gilmour'),
('Rain Bird'),
('Orbit'),
('Melnor'),
('Ortho'),
('Spectracide'),
('Safer Brand'),
('Miracle-Gro'),
('Espoma'),
('FoxFarm'),
('Keter'),
('Hampton Bay'),
('PatioSense'),
('Palram'),
('Monticello'),
('Rion'),
('Scotts'),
('Roundup'),
('Preen');

DECLARE @PlantsId INT = (SELECT Id FROM Categories WHERE Name = 'Plants');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PlantsId
FROM Brands
WHERE Name IN ('Proven Winners', 'Bonnie Plants', 'Burpee');

DECLARE @SeedsId INT = (SELECT Id FROM Categories WHERE Name = 'Seeds');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SeedsId
FROM Brands
WHERE Name IN ('Park Seed', 'Johnny''s Selected Seeds', 'Baker Creek Heirloom Seeds');

DECLARE @GardenToolsId INT = (SELECT Id FROM Categories WHERE Name = 'Garden Tools');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @GardenToolsId
FROM Brands
WHERE Name IN ('Fiskars', 'Corona', 'Ames', 'True Temper');

DECLARE @OutdoorDecorId INT = (SELECT Id FROM Categories WHERE Name = 'Outdoor Decor');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @OutdoorDecorId
FROM Brands
WHERE Name IN ('Evergreen Enterprises', 'Alpine Corporation', 'Achla Designs');

DECLARE @WateringEquipmentId INT = (SELECT Id FROM Categories WHERE Name = 'Watering Equipment');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WateringEquipmentId
FROM Brands
WHERE Name IN ('Gilmour', 'Rain Bird', 'Orbit', 'Melnor');

DECLARE @PestControlId INT = (SELECT Id FROM Categories WHERE Name = 'Pest Control');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PestControlId
FROM Brands
WHERE Name IN ('Ortho', 'Spectracide', 'Safer Brand');

DECLARE @SoilAndFertilizersId INT = (SELECT Id FROM Categories WHERE Name = 'Soil and Fertilizers');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @SoilAndFertilizersId
FROM Brands
WHERE Name IN ('Miracle-Gro', 'Espoma', 'FoxFarm');

DECLARE @GardenFurnitureId INT = (SELECT Id FROM Categories WHERE Name = 'Garden Furniture');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @GardenFurnitureId
FROM Brands
WHERE Name IN ('Keter', 'Hampton Bay', 'PatioSense');

DECLARE @GreenhousesId INT = (SELECT Id FROM Categories WHERE Name = 'Greenhouses');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @GreenhousesId
FROM Brands
WHERE Name IN ('Palram', 'Monticello', 'Rion');

DECLARE @LandscapingId INT = (SELECT Id FROM Categories WHERE Name = 'Landscaping');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @LandscapingId
FROM Brands
WHERE Name IN ('Scotts', 'Roundup', 'Preen');

-- Insert Brands for Health Subcategories
INSERT INTO Brands (Name)
VALUES
('Nature''s Bounty'),
('Centrum'),
('Garden of Life'),
('Now Foods'),
('Colgate'),
('Crest'),
('Oral-B'),
('Philips Sonicare'),
('Band-Aid'),
('Neosporin'),
('Curad'),
('Medline'),
('TheraBand'),
('Gaiam'),
('Manduka'),
('TriggerPoint'),
('Nature''s Way'),
('Traditional Medicinals'),
('Gaia Herbs'),
('Omron'),
('Withings'),
('iHealth'),
('Johnson & Johnson'),
('First Aid Only'),
('Adventure Medical Kits'),
('Drive Medical'),
('Invacare'),
('Mayo Clinic'),
('WebMD'),
('Harvard Health Publications');

DECLARE @VitaminsAndSupplementsId INT = (SELECT Id FROM Categories WHERE Name = 'Vitamins and Supplements');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @VitaminsAndSupplementsId
FROM Brands
WHERE Name IN ('Nature''s Bounty', 'Centrum', 'Garden of Life', 'Now Foods');

DECLARE @PersonalCareHealthId INT = (SELECT Id FROM Categories WHERE Name = 'Personal Care' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Health'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @PersonalCareHealthId
FROM Brands
WHERE Name IN ('Colgate', 'Crest', 'Oral-B', 'Philips Sonicare');

DECLARE @MedicalSuppliesId INT = (SELECT Id FROM Categories WHERE Name = 'Medical Supplies');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MedicalSuppliesId
FROM Brands
WHERE Name IN ('Band-Aid', 'Neosporin', 'Curad', 'Medline');

DECLARE @FitnessEquipmentHealthId INT = (SELECT Id FROM Categories WHERE Name = 'Fitness Equipment' AND ParentCategoryId = (SELECT Id FROM Categories WHERE Name = 'Health'));
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FitnessEquipmentHealthId
FROM Brands
WHERE Name IN ('Bowflex', 'NordicTrack', 'Peloton', 'Rogue Fitness');

DECLARE @WellnessProductsId INT = (SELECT Id FROM Categories WHERE Name = 'Wellness Products');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @WellnessProductsId
FROM Brands
WHERE Name IN ('TheraBand', 'Gaiam', 'Manduka', 'TriggerPoint');

DECLARE @AlternativeMedicineId INT = (SELECT Id FROM Categories WHERE Name = 'Alternative Medicine');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @AlternativeMedicineId
FROM Brands
WHERE Name IN ('Nature''s Way', 'Traditional Medicinals', 'Gaia Herbs');

DECLARE @HealthMonitorsId INT = (SELECT Id FROM Categories WHERE Name = 'Health Monitors');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @HealthMonitorsId
FROM Brands
WHERE Name IN ('Omron', 'Withings', 'iHealth', 'Fitbit');

DECLARE @FirstAidId INT = (SELECT Id FROM Categories WHERE Name = 'First Aid');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @FirstAidId
FROM Brands
WHERE Name IN ('Johnson & Johnson', 'First Aid Only', 'Adventure Medical Kits');

DECLARE @MobilityAidsId INT = (SELECT Id FROM Categories WHERE Name = 'Mobility Aids');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @MobilityAidsId
FROM Brands
WHERE Name IN ('Drive Medical', 'Medline', 'Invacare');

DECLARE @HealthBooksId INT = (SELECT Id FROM Categories WHERE Name = 'Health Books');
INSERT INTO BrandCategory (BrandId, CategoryId)
SELECT Id, @HealthBooksId
FROM Brands
WHERE Name IN ('Mayo Clinic', 'WebMD', 'Harvard Health Publications');