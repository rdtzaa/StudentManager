USE StudentDB;
GO

-- Penyesuaian database latihan yang sudah ada; data lama tetap dipertahankan.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF COL_LENGTH('dbo.Students', 'Gender') IS NULL
    ALTER TABLE dbo.Students ADD Gender VARCHAR(20) NULL;
GO

-- Gender data lama yang tidak ada dalam contoh modul diisi melalui aplikasi.
UPDATE dbo.Students SET Gender = '' WHERE Gender IS NULL;
ALTER TABLE dbo.Students ALTER COLUMN Gender VARCHAR(20) NOT NULL;

UPDATE dbo.Students SET Jurusan = '' WHERE Jurusan IS NULL;
ALTER TABLE dbo.Students ALTER COLUMN Jurusan VARCHAR(100) NOT NULL;

UPDATE dbo.Students SET Gender = 'Laki-laki'
WHERE NIM = '23001' AND Nama = 'Budi Santoso' AND Gender = '';
UPDATE dbo.Students SET Gender = 'Perempuan'
WHERE NIM = '23002' AND Nama = 'Siti Aminah' AND Gender = '';

-- Tambahkan contoh modul hanya jika NIM tersebut belum ada.
INSERT INTO dbo.Students (NIM, Nama, Jurusan, Gender, Email)
SELECT sample.NIM, sample.Nama, sample.Jurusan, sample.Gender, sample.Email
FROM (VALUES
    ('23001', 'Budi Santoso', 'Informatika', 'Laki-laki', 'budi@gmail.com'),
    ('23002', 'Siti Aminah', 'Sistem Informasi', 'Perempuan', 'siti@gmail.com'),
    ('23003', 'Andi Wijaya', 'Informatika', 'Laki-laki', 'andi@gmail.com'),
    ('23004', 'Rina Sari', 'Sistem Informasi', 'Perempuan', 'rina@gmail.com')
) AS sample(NIM, Nama, Jurusan, Gender, Email)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Students WHERE NIM = sample.NIM);

COMMIT TRANSACTION;
