# ARC
Sebuah alat yang dapat membuat kita berinteraksi langsung dengan objek 3D yang ada di dalam AR melalui android

# Kelompok 1 (Ar-Cademia)
1. Ararya Pandya Arjana (X MIPA 3)
2. Rifky Bujana Bisri (X MIPA 3)
3. Muhammad Rafly (X MIPA 3)
4. Hafizhuddin Rasyid (XI MIPA 4)
5. Ghefira Airashin (XI MIPA 3)

# Deskripsi Robot
  Alat yang dibuat oleh kelompok kami merupakan penggabungan dari fungsi XR. Robot ini
dapat membantu kegiatan manusia dalam berbagai bidang, sperti Pendidikan, pekerjaan,
kedokteran, dll. Di masa depan hal ini dapat sangat membantu manusia seperti
  memvisualisasikan pelajaran yang sedang di pelajari, seperti sebuah atom. Jika kita dapat
memvisualisasikan atom saat kita sedang belajar kimia, kita dapat mengerti lebih baik tentang
atom tersebut. Serta kita dapat “berinteraksi langsung” dengan objek yang sedang dipelajari
tersebut.

# Fungsi Utama
1. Penggambaran objek secara real
2. Menambah interversi digital
3. Berinteraksi dengan objek 3D
4. Memberikan informasi yang dibutuhkan dari objek

# Manfaat/Keunggulan
1. Mempermudah pemaparan baik dalam presentasi maupun pembelajaran
2. Memberikan penggambaran objek digital secara real
3. Menjadikan presentasi/pembelajaran lebih menarik
4. Dapat berinteraksi langsung dengan objek yang sedang dipelajari

# Perbandingan dengan sistem sekarang
1. Alat kami menggabungkan fitur dari VR (Virtual Reality) dan AR (Augmented
Reality) yang sekarang ini masih jarang pengembangan nya.
2. Dengan pemanfaatan HP(handphone) sebagai interface menjadikan alat jauh lebih
terjangkau bagi umum jika dibandingkan dengan VR gear lain yang harganya cukup
mahal.
3. Alat ini membuat kita dapat berinteraksi dengan objek 3D dengan tangan kita,
sedangkan sekarang mayoritas perangkat VR dan AR masih menggunakan remote

# Cara Kerja
# Hardware :
1. Membaca data dari sensor flex di setiap jari untuk mengetahui lekukan/rotasi jari
2. Membaca sensor gyro untuk mengetahui rotasi tangan
3. Mengkalkulasikan hasil dari sensor flex dan gyro untuk mendapatkan gerakan dan
posisi tangan serta jari
4. Mengirim data hasil kalkulasi ke smartphone pengguna dengan bluethoot module
HC05

# Software :
Cara ARCore Bekerja: 
  Saat ponsel Anda bergerak di seluruh dunia, ARCore menggunakan proses yang disebut odometry dan pemetaan bersamaan, atau COM, untuk   memahami di mana ponsel relatif terhadap dunia di sekitarnya. ARCore mendeteksi fitur yang berbeda secara visual dalam gambar kamera      yang ditangkap yang disebut titik fitur dan menggunakan titik-titik ini untuk menghitung perubahan lokasinya. Informasi visual  dikombinasikan dengan pengukuran inersia dari IMU perangkat untuk memperkirakan pose (posisi dan orientasi) kamera relatif terhadap   dunia dari waktu ke waktu.

  lalu berdasarkan data yang didapat dari Hardware kita dapat menentukan posisi dan gerakan tangan. Lalu membuat objek bergerak atau melakukan sesuatu berdasarkan interaksi dari tangan
  
# Future Development
1. Memperhalus interface alat
2. Memperkecil module elektronik pada sarung tangan

# Bahan
1. Flex sensor 5 buah (handmade/beli)
2. Gyro sensor (MPU-6050)
3. Sarung tangan Panjang
4. Arduino UNO + Shield
5. Kabel pita 1 meter
6. Kabel Jumper masing-masing 20 (male-female; male-male; female-female)
7. Kabel AWG 1 meter
8. Besi blackhousing female + kepala pin
9. Baterai Li-Po
10. Connector baterai ke Arduino
11. VR box
12. Bluetooth module
