using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

namespace WindowsPhoneGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int[,] labirent = new int[15, 25];
        int[,] labirent2 = new int[15, 25];
        int[,] labirent3 = new int[15, 25];
        int[,] labirent4 = new int[15, 25];

        int[,] labirent_ilk = new int[15, 25]{
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,1,0,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
                        {0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,1,0,0},
                        {0,1,0,0,0,0,0,0,1,1,1,1,1,1,0,1,0,0,0,0,1,1,1,0,0},
                        {0,1,1,1,1,1,1,0,0,0,0,1,0,1,0,1,0,1,0,0,0,0,1,1,1},
                        {0,1,0,0,0,0,0,0,0,1,0,1,0,1,1,1,1,1,1,1,0,0,1,0,0},
                        {0,1,0,1,1,0,1,1,1,0,0,1,0,0,1,0,0,0,0,1,0,0,1,0,0},
                        {0,1,0,0,0,0,1,0,1,1,1,1,0,1,1,0,1,0,0,1,1,1,1,0,0},
                        {0,1,1,1,1,1,1,0,0,1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0},
                        {0,1,0,0,0,1,0,0,1,1,1,0,0,1,0,1,1,1,1,1,1,0,0,1,0},
                        {0,1,1,1,0,0,0,0,1,0,1,0,0,1,0,1,0,0,0,0,1,1,1,1,1},
                        {0,1,0,1,0,1,1,0,1,0,1,1,1,1,1,1,1,1,1,0,0,0,0,1,0},
                        {0,1,0,0,0,1,0,0,1,1,1,0,1,0,0,0,1,0,1,0,0,0,0,0,0},
                        {0,1,1,1,1,1,0,1,1,0,0,0,1,0,1,0,1,1,1,1,1,1,1,1,0},
                        {0,1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0}
                       };

        int[,] koordinat = new int[200, 3];     //  x:0  y:1

        int[,] gidebilecegi_yer = new int[200, 4];      //0:ileri   1:sað   2:sol

        int[,] fare_oncelik = new int[3, 3];        //10:ileri    11:sað     12:sol

        int koordinat_x ;
        int koordinat_y ;

        int ilk_x;
        int ilk_y;

        int yon ;            //2:yukarý   4:sol   6:sað   8:aþaðý
        int ilk_yon;

        int a=0;

        int gidebilecegi_yer_x = 0;
        int koordinat_satir = 0;
        int gittigi_yer_sayac = 1;

        int sayac = 1;

        int fare1_mesafe=0;
        int fare2_mesafe=0;
        int fare3_mesafe=0;

        ///////////////////////////////////////////////////////////////////////////////////////////

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D duvar;
        Texture2D fare_cizgi;
        Texture2D kapi;

        Texture2D fare;
        Vector2 fare_yon;
        Vector2 ilk_fare_yon;
        Vector2 fare_agirlik_merkezi;
        Rectangle boyut;

        float fare_buyukluk = 0.1f;
        float fare_donus = 3.2f;

        SpriteFont textFont;
        Vector2 textPosition;
        string TEXT = "";


        public void write_temporarytext()
        {


            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.DirectoryExists("database"))
            {
                storage.CreateDirectory("database");
            }
            else if (storage.FileExists("database\\temporary.txt"))
            {
                storage.DeleteFile("database\\temporary.txt");
            }

            StreamWriter register = new StreamWriter(new IsolatedStorageFileStream("database\\temporary.txt", FileMode.OpenOrCreate, storage));

            for (int i = 0; i < 15; i++)
            {

                for (int j = 0; j < 25; j++)
                {
                    if (j != 24)
                        register.Write(labirent_ilk[i, j] + ",");
                    if (j == 24)
                        register.Write(labirent_ilk[i, j] + "\n");

                }

            }

            register.Close();
        }

        public void read_temporarytext()
        {
            string[] value = new string[3];

            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.DirectoryExists("database"))
            {
                storage.CreateDirectory("database");
            }
            else if (!storage.FileExists("database\\temporary.txt"))
            {
                StreamWriter register = new StreamWriter(new IsolatedStorageFileStream("database\\temporary.txt", FileMode.OpenOrCreate, storage));

                register.Close();
            }
            else
            {
                IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("database\\temporary.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        //for each row
                        for (int i = 0; i < 15; i++)
                        {
                            //read in the line
                            string myLine = reader.ReadLine();
                            //take out the commas
                            string[] row = myLine.Split(',');
                            //sayi = row.Length;//

                            //convert to string to ints
                            //and feed back into array
                            int[] nRow = new int[row.Length];
                            for (int r = 0; r < row.Length; r++)
                            {
                                nRow[r] = Convert.ToInt32(row[r]);
                                labirent[i, r] = nRow[r];
                            }

                        }
                    }


                }
            }
        }

        void ilk_deger()
        {

            fare_oncelik[0, 0] = 10;
            fare_oncelik[0, 1] = 11;
            fare_oncelik[0, 2] = 12;

            fare_oncelik[1, 0] = 12;
            fare_oncelik[1, 1] = 10;
            fare_oncelik[1, 2] = 11;

            fare_oncelik[2, 0] = 11;
            fare_oncelik[2, 1] = 12;
            fare_oncelik[2, 2] = 10;

            for (int k = 0; k < 200; k++)
            {
                for (int l = 0; l < 4; l++)
                {
                    gidebilecegi_yer[k, l] = -1;
                }
            }
            for (int k = 0; k < 200; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    koordinat[k, l] = -1;
                }
            }
        }

        void mesafe_bul()
        {
            int en_buyuk = labirent2[0,0];
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (en_buyuk < labirent2[k, l])
                    {
                        en_buyuk = labirent2[k, l];
                    }
                }
            }
            fare1_mesafe = en_buyuk;

            en_buyuk = labirent3[0, 0];
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (en_buyuk < labirent3[k, l])
                    {
                        en_buyuk = labirent3[k, l];
                    }
                }
            }
            fare2_mesafe = en_buyuk;

            en_buyuk = labirent4[0, 0];
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (en_buyuk < labirent4[k, l])
                    {
                        en_buyuk = labirent4[k, l];
                    }
                }
            }
            fare3_mesafe = en_buyuk;
        }

        void gidebilecegi_yer_son_eleman_hesapla()
        {
            int temp = 0;
            for (int k = 0; k < 200; k++)
            {
                temp += gidebilecegi_yer[k, 0];
                temp += gidebilecegi_yer[k, 1];
                temp += gidebilecegi_yer[k, 2];
                gidebilecegi_yer[k, 3] = temp;
                temp = 0;
            }
        }

        void gidebilecegi_yer_guncelle()
        {
            for (int k = 0; k < 200; k++)
            {
                if (koordinat[k, 2] == 2)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] - 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                }
                if (koordinat[k, 2] == 4)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1] - 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                }
                if (koordinat[k, 2] == 6)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] - 1, koordinat[k, 0]];
                }
                if (koordinat[k, 2] == 8)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                }
            }   
        }

        void gidebilecegi_yer_guncelle2()
        {
            for (int k = 0; k < 200; k++)
            {
                if (koordinat[k, 2] == 2)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1]- 1, koordinat[k, 0] ];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] , koordinat[k, 0] + 1];
                }
                if (koordinat[k, 2] == 4)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] -1, koordinat[k, 0]];
                }
                if (koordinat[k, 2] == 6)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1]- 1, koordinat[k, 0] ];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                }
                if (koordinat[k, 2] == 8)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] , koordinat[k, 0] +1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1]+ 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1], koordinat[k, 0]  - 1];
                }
            }
        }

        void gidebilecegi_yer_guncelle3()
        {
            for (int k = 0; k < 200; k++)
            {
                if (koordinat[k, 2] == 2)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] , koordinat[k, 0] +1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] - 1, koordinat[k, 0]];
                }
                if (koordinat[k, 2] == 4)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1]- 1, koordinat[k, 0] ];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                }
                if (koordinat[k, 2] == 6)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1] - 1, koordinat[k, 0]];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                }
                if (koordinat[k, 2] == 8)
                {
                    gidebilecegi_yer[k + 1, 0] = labirent[koordinat[k, 1], koordinat[k, 0] - 1];
                    gidebilecegi_yer[k + 1, 1] = labirent[koordinat[k, 1], koordinat[k, 0] + 1];
                    gidebilecegi_yer[k + 1, 2] = labirent[koordinat[k, 1] + 1, koordinat[k, 0]];
                }
            }
        }

        void atla()
        {
            for (int k = 199; k >= 0; k--)
            {
                if (gidebilecegi_yer[k, 3] == 1 || gidebilecegi_yer[k, 3] == 2)
                {
                    koordinat_y = koordinat[k - 1, 1];
                    koordinat_x = koordinat[k - 1, 0];
                    yon = koordinat[k - 1, 2];
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                    break;
                }

            }
        }

        bool kontrol()
        {
            if (ilk_x == 1 && ilk_y == 14)
            {
                if ((koordinat_y == 14 && koordinat_x == 7) ||
                (koordinat_y == 14 && koordinat_x == 12) ||
                (koordinat_y == 4 && koordinat_x == 24) ||
                (koordinat_y == 10 && koordinat_x == 24))
                {
                    return true;
                }
                else return false;
            }

            else if (ilk_x == 7 && ilk_y == 14)
            {
                if ((koordinat_y == 14 && koordinat_x == 1) ||
                (koordinat_y == 14 && koordinat_x == 12) ||
                (koordinat_y == 4 && koordinat_x == 24) ||
                (koordinat_y == 10 && koordinat_x == 24))
                {
                    return true;
                }
                else return false;
            }
            else if (ilk_x == 12 && ilk_y == 14)
            {
                if ((koordinat_y == 14 && koordinat_x == 1) ||
                (koordinat_y == 14 && koordinat_x == 7) ||
                (koordinat_y == 4 && koordinat_x == 24) ||
                (koordinat_y == 10 && koordinat_x == 24))
                {
                    return true;
                }
                else return false;
            }
            else if (ilk_x == 24 && ilk_y == 4)
            {
                if ((koordinat_y == 14 && koordinat_x == 1) ||
                (koordinat_y == 14 && koordinat_x == 7) ||
                (koordinat_y == 14 && koordinat_x == 12) ||
                (koordinat_y == 10 && koordinat_x == 24))
                {
                    return true;
                }
                else return false;
            }
            else if (ilk_x == 24 && ilk_y == 10)
            {
                if ((koordinat_y == 14 && koordinat_x == 1) ||
                (koordinat_y == 14 && koordinat_x == 7) ||
                (koordinat_y == 14 && koordinat_x == 12) ||
                (koordinat_y == 4 && koordinat_x == 24))
                {
                    return true;
                }
                else return false;
            }
            return false;
        }

        void menu()
        {
            switch (yon)
            {
                case 2:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y - 1, koordinat_x];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y, koordinat_x + 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y, koordinat_x - 1];

                            if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent2[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y - 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent2[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 6;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 4;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;


                case 4:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y, koordinat_x - 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y - 1, koordinat_x];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y + 1, koordinat_x];

                            if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent2[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x - 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent2[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 2;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 8;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 6:
                    {
                        if (!kontrol())
                        {

                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y, koordinat_x + 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y + 1, koordinat_x];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y - 1, koordinat_x];

                            if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;


                                if (labirent2[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x + 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent2[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 8;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 2;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 8:
                    {
                        if (!kontrol())
                        {

                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y + 1, koordinat_x];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y, koordinat_x - 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y, koordinat_x + 1];

                            if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent2[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y + 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[0, 0] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent2[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 4;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[0, 1] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 6;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[0, 2] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent2[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent2[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;
            }
        }

        void menu2()
        {
            switch (yon)
            {
                case 2:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y , koordinat_x -1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y-1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y, koordinat_x + 1];

                            if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent3[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x - 1;
                                yon = 4;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent3[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 2;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 6;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle2();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;


                case 4:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y +1 , koordinat_x];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y , koordinat_x-1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y - 1, koordinat_x];

                            if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent3[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y + 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                yon = 8;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent3[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 4;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 2;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle2();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 6:
                    {
                        if (!kontrol())
                        {

                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y-1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y , koordinat_x+1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y + 1, koordinat_x];

                            if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;


                                if (labirent3[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y - 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                yon = 2;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent3[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 6;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 8;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle2();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 8:
                    {
                        if (!kontrol())
                        {

                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y , koordinat_x+1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y+1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y, koordinat_x - 1];

                            if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent3[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x + 1;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                yon = 6;
                                koordinat[koordinat_satir, 2] = yon;
                            }
                            else if (fare_oncelik[1, 0] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent3[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 8;
                                    koordinat[koordinat_satir, 2] = yon;
                                }
                                else if (fare_oncelik[1, 1] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 4;
                                        koordinat[koordinat_satir, 2] = yon;
                                    }
                                    else if (fare_oncelik[1, 2] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent3[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent3[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle2();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();
                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;
            }
        }

        void menu3()
        {
            switch (yon)
            {
                case 2:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y, koordinat_x +1 ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y , koordinat_x-1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y-1, koordinat_x ];

                            if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent4[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x + 1;
                                yon = 6;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent4[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 4;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 2;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle3();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;


                case 4:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y-1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y+1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y , koordinat_x-1];

                            if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent4[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y - 1;
                                yon = 2;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent4[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 8;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x - 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 4;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle3();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 6:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y +1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y- 1, koordinat_x ];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y, koordinat_x+1];

                            if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent4[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_y = koordinat_y + 1;
                                yon = 8;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent4[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_y = koordinat_y - 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 2;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_x = koordinat_x + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 6;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle3();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;

                case 8:
                    {
                        if (!kontrol())
                        {
                            gidebilecegi_yer[gidebilecegi_yer_x, 0] = labirent[koordinat_y, koordinat_x - 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 1] = labirent[koordinat_y, koordinat_x + 1];
                            gidebilecegi_yer[gidebilecegi_yer_x, 2] = labirent[koordinat_y + 1, koordinat_x];

                            if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 1)
                            {
                                gidebilecegi_yer[gidebilecegi_yer_x, 0] = 0;

                                if (labirent4[koordinat_y, koordinat_x] == 0)
                                {
                                    labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                }
                                labirent[koordinat_y, koordinat_x] = 0;

                                koordinat_x = koordinat_x - 1;
                                yon = 4;
                                koordinat[koordinat_satir, 0] = koordinat_x;
                                koordinat[koordinat_satir, 1] = koordinat_y;
                                koordinat[koordinat_satir, 2] = yon;

                            }
                            else if (fare_oncelik[2, 0] == 11 && gidebilecegi_yer[gidebilecegi_yer_x, 0] == 0)
                            {
                                if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 1)
                                {
                                    gidebilecegi_yer[gidebilecegi_yer_x, 1] = 0;

                                    if (labirent4[koordinat_y, koordinat_x] == 0)
                                    {
                                        labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                    }
                                    labirent[koordinat_y, koordinat_x] = 0;

                                    koordinat_x = koordinat_x + 1;
                                    koordinat[koordinat_satir, 0] = koordinat_x;
                                    koordinat[koordinat_satir, 1] = koordinat_y;
                                    yon = 6;
                                    koordinat[koordinat_satir, 2] = yon;

                                }
                                else if (fare_oncelik[2, 1] == 12 && gidebilecegi_yer[gidebilecegi_yer_x, 1] == 0)
                                {
                                    if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 1)
                                    {
                                        gidebilecegi_yer[gidebilecegi_yer_x, 2] = 0;

                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;

                                        koordinat_y = koordinat_y + 1;
                                        koordinat[koordinat_satir, 0] = koordinat_x;
                                        koordinat[koordinat_satir, 1] = koordinat_y;
                                        yon = 8;
                                        koordinat[koordinat_satir, 2] = yon;

                                    }
                                    else if (fare_oncelik[2, 2] == 10 && gidebilecegi_yer[gidebilecegi_yer_x, 2] == 0)
                                    {
                                        if (labirent4[koordinat_y, koordinat_x] == 0)
                                        {
                                            labirent4[koordinat_y, koordinat_x] = gittigi_yer_sayac; gittigi_yer_sayac = gittigi_yer_sayac + 1;
                                        }
                                        labirent[koordinat_y, koordinat_x] = 0;
                                        gidebilecegi_yer_guncelle3();
                                        gidebilecegi_yer_son_eleman_hesapla();
                                        atla();

                                    }
                                }
                            }
                            gidebilecegi_yer_x = gidebilecegi_yer_x + 1;
                            koordinat_satir = koordinat_satir + 1;
                        }
                    }
                    break;
            }
        }

        void sifirla()
        {
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    labirent[k, l] = labirent_ilk[k, l];
                }
            }

            for (int k = 0; k < 200; k++)
            {
                for (int l = 0; l < 4; l++)
                {
                    gidebilecegi_yer[k, l] = -1;
                }
            }
            for (int k = 0; k < 200; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    koordinat[k, l] = -1;
                }
            }

            fare_yon = ilk_fare_yon;
            koordinat_x = ilk_x;
            koordinat_y = ilk_y;

            yon = ilk_yon;

            gidebilecegi_yer_x = 0;
            koordinat_satir = 0;
            gittigi_yer_sayac = 1;

            koordinat[koordinat_satir, 0] = koordinat_x;
            koordinat[koordinat_satir, 1] = koordinat_y;
            koordinat[koordinat_satir, 2] = yon;
        }


        public Game1() //constracturator
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(180000);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
                   
        }

        protected override void Initialize()  //1
        {
             //TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()  //2--> imaj, ses, yazý v.s gibi içerikler yüklenerek oyun için hazýr duruma getirilir.
        {
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            //if (!storage.DirectoryExists("database"))
            //{
            //    storage.CreateDirectory("database");
            //}
            //if (!storage.FileExists("database\\options.txt"))
            //{
            //    StreamWriter register = new StreamWriter(new IsolatedStorageFileStream("database\\options.txt", FileMode.OpenOrCreate, storage));
            //    //for (int k = 0; k < 15; k++)
            //    //{
            //    //    for (int l = 0; l < 25; l++)
            //    //    {
                        
            //            register.WriteLine(labirent[1, 1]);
            //    //    }
            //    //}

            //    //register.Close();
            //}
            

            //StreamReader reader = null;
            //reader = new StreamReader(new IsolatedStorageFileStream("database\\options.txt", FileMode.Open, storage));
            ////for (int k = 0; k < 15; k++)
            ////{
            ////     for (int l = 0; l < 25; l++)
            ////     {
            //string a;
            //            a = reader.ReadLine();
            //            labirent2[0, 0] = Int32.Parse(a);
            ////     }
            ////}

            //reader.Close();
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            write_temporarytext();
            read_temporarytext();


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Viewport viewPort = graphics.GraphicsDevice.Viewport;

            duvar = Content.Load<Texture2D>("duvar");
            kapi = Content.Load<Texture2D>("kirmizi");
            fare_cizgi = Content.Load<Texture2D>("çizgi");
                        
            fare = Content.Load<Texture2D>("fare_resim");
            fare_agirlik_merkezi = new Vector2(fare.Width/2, fare.Height/2);

            textFont = this.Content.Load<SpriteFont>("CarGameFont");
            textPosition = new Vector2(240, 170);
           
            ilk_deger();

            
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()  //5
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)  //3-->istenilen deðiþiklikleri, matematiksel hesaplamalarý, yani oyunun iþ mantýðýný buraya yazýyoruz, nesne yerlerinin/koordinatlarýnýn zamanla deðiþmesi,v.s
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0 && a==0 )
            {
                ++a;
                TouchLocation t1 = touchCollection[0];

                int x = (int)(t1.Position.X);
                int y = (int)(t1.Position.Y);
                if (x < 64 && x > 32 && y < 480 && y > 448)
                {
                    fare_yon = new Vector2(48, 464);
                    ilk_fare_yon = new Vector2(48, 464);
                    koordinat_x = 1;
                    koordinat_y = 14;

                    ilk_x=1;
                    ilk_y=14;

                    yon = 2;
                    ilk_yon = 2;
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                }
                
                if (x < 256 && x > 224 && y < 480 && y > 448)
                {
                    fare_yon = new Vector2(240, 464);
                    ilk_fare_yon = new Vector2(240, 464);
                    koordinat_x = 7;
                    koordinat_y = 14;

                    ilk_x=7;
                    ilk_y=14;

                    yon = 2;
                    ilk_yon = 2;
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                }
                if (x < 416 && x > 384 && y < 480 && y > 448)
                {
                    fare_yon = new Vector2(400, 464);
                    ilk_fare_yon = new Vector2(400, 464);
                    koordinat_x = 12;
                    koordinat_y = 14;

                    ilk_x=12;
                    ilk_y=14;

                    yon = 2;
                    ilk_yon = 2;
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                }
                if (x < 800 && x > 768 && y < 160 && y > 128)
                {
                    fare_yon = new Vector2(784, 144);
                    ilk_fare_yon = new Vector2(784, 144);
                    koordinat_x = 24;
                    koordinat_y = 4;

                    ilk_x=24;
                    ilk_y=4;

                    yon = 4;
                    ilk_yon = 4;
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                }
                if (x < 800 && x > 768 && y < 352 && y > 320)
                {
                    fare_yon = new Vector2(784, 336);
                    ilk_fare_yon = new Vector2(784, 336);
                    koordinat_x = 24;
                    koordinat_y = 10;

                    ilk_x=24;
                    ilk_y=10;

                    yon = 4;
                    ilk_yon = 4;
                    koordinat[koordinat_satir, 0] = koordinat_x;
                    koordinat[koordinat_satir, 1] = koordinat_y;
                    koordinat[koordinat_satir, 2] = yon;
                }
                for (int i = 0; i < 199; i++)
                {
                    menu();
                }
                sifirla();
                for (int i = 0; i < 199; i++)
                {
                    menu2();
                }
                sifirla();
                for (int i = 0; i < 199; i++)
                {
                    menu3();
                }
                sifirla();
                mesafe_bul();
                TEXT = string.Concat("FARE 1 in gittigi mesafe : " + fare1_mesafe + "\n\n" +
                                     "FARE 2 in gittigi mesafe : " + fare2_mesafe + "\n\n" +
                                     "FARE 3 in gittigi mesafe : " + fare3_mesafe + "\n\n" );
            }
            
            if(a == 1)
            {
                cizdirme(labirent2);
                if (!labda_sayac_var_mi(labirent2))
                {
                    a = 2;
                    sayac = 1;
                    sifirla();
                }
            }
            if (a == 2)
            {
                cizdirme(labirent3);
                if (!labda_sayac_var_mi(labirent3))
                {
                    a = 3;
                    sayac = 1;
                    sifirla();
                }
            }
            if (a == 3)
            {
                cizdirme(labirent4);
                if (!labda_sayac_var_mi(labirent4))
                {
                    a = 4;
                    fare_yon = new Vector2(0 ,0 );
                    fare_buyukluk = 0.0f;
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)  
        {
            GraphicsDevice.Clear(Color.GreenYellow);
            spriteBatch.Begin();

            if(a==0 || a==1 || a==2 || a==3)
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (labirent_ilk[k, l] == 0)
                    {
                            boyut = new Rectangle(l * 32, k * 32, 32, 32);
                            spriteBatch.Draw(duvar, boyut, Color.Blue);
                       
                    }
                    if ((k == 14 || l == 24) && labirent_ilk[k, l] == 1)
                    {
                        boyut = new Rectangle(l * 32, k * 32, 32, 32);
                        spriteBatch.Draw(kapi, boyut, Color.Sienna);
                    }
                }
            }
                       
            if(a==1)
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (labirent2[k, l] <= sayac-1 && labirent2[k, l] != 0)
                    {
                        Rectangle boyut2 = new Rectangle(l * 32 + 13, k * 32 + 13, 4, 4);
                        //Rectangle boyut2 = new Rectangle((int)(fare_yon.X), (int)(fare_yon.Y), 5, 5);
                        spriteBatch.Draw(fare_cizgi, boyut2, Color.Blue);
                    }
                }
            }
            if (a == 2)
                for (int k = 0; k < 15; k++)
                {
                    for (int l = 0; l < 25; l++)
                    {
                        if (labirent3[k, l] <= sayac - 1 && labirent3[k, l] != 0)
                        {
                            Rectangle boyut2 = new Rectangle(l * 32 + 13, k * 32 + 13, 4, 4);
                            //Rectangle boyut2 = new Rectangle((int)(fare_yon.X), (int)(fare_yon.Y), 5, 5);
                            spriteBatch.Draw(fare_cizgi, boyut2, Color.Blue);
                        }
                    }
                }
            if (a == 3)
                for (int k = 0; k < 15; k++)
                {
                    for (int l = 0; l < 25; l++)
                    {
                        if (labirent4[k, l] <= sayac - 1 && labirent4[k, l] != 0)
                        {
                            Rectangle boyut2 = new Rectangle(l * 32 + 13, k * 32 + 13, 4, 4);
                            //Rectangle boyut2 = new Rectangle((int)(fare_yon.X), (int)(fare_yon.Y), 5, 5);
                            spriteBatch.Draw(fare_cizgi, boyut2, Color.Blue);
                        }
                    }
                }

            
                spriteBatch.Draw(fare, fare_yon, null, Color.White, fare_donus, fare_agirlik_merkezi, fare_buyukluk, SpriteEffects.None, 0);
                spriteBatch.End();
            

            if (a == 4)
            {
                GraphicsDevice.Clear(Color.BlueViolet);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.DrawString(textFont, TEXT, textPosition, Color.Black);
                spriteBatch.End();
            }

            
            
            //spriteBatch.Begin();
            //Rectangle boyut2 = new Rectangle((int)(fare_yon.X), (int)(fare_yon.Y), 20, 20);
            //Rectangle boyut2 = new Rectangle(22, 22, 3, 4);
            //spriteBatch.Draw(fare_cizgi, boyut2, Color.Blue);
            //spriteBatch.End();

            

            base.Draw(gameTime);
        }

        bool labda_sayac_var_mi(int[,] gelen_labirent)
        {
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (gelen_labirent[k, l] == sayac)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void cizdirme(int[,] gelen_labirent)
        {
            for (int k = 0; k < 15; k++)
            {
                for (int l = 0; l < 25; l++)
                {
                    if (gelen_labirent[k, l] == sayac)
                    {
                        if (fare_yon.Y < (k * 32 + 15))
                        {
                            if (Math.Sqrt(Math.Pow((k * 32 + 15) - fare_yon.Y, 2) + Math.Pow(fare_yon.X - (l * 32 + 15), 2)) > 32)
                            {
                                if (gelen_labirent[k - 1, l] < sayac && gelen_labirent[k - 1, l] != 0)
                                {
                                    fare_yon.Y = ((k - 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k + 1, l] < sayac && gelen_labirent[k + 1, l] != 0)
                                {
                                    fare_yon.Y = ((k + 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k, l - 1] < sayac && gelen_labirent[k, l - 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l - 1) * 32 + 15);
                                }
                                if (gelen_labirent[k, l + 1] < sayac && gelen_labirent[k, l + 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l + 1) * 32 + 15);
                                }

                            }
                            else
                            {
                                fare_yon.Y += 1; fare_donus = 0.0f;
                            }
                        }
                        if (fare_yon.Y > (k * 32 + 15))
                        {
                            if (Math.Sqrt(Math.Pow((k * 32 + 15) - fare_yon.Y, 2) + Math.Pow(fare_yon.X - (l * 32 + 15), 2)) > 32)
                            {
                                if (gelen_labirent[k - 1, l] < sayac && gelen_labirent[k - 1, l] != 0)
                                {
                                    fare_yon.Y = ((k - 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k + 1, l] < sayac && gelen_labirent[k + 1, l] != 0)
                                {
                                    fare_yon.Y = ((k + 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k, l - 1] < sayac && gelen_labirent[k, l - 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l - 1) * 32 + 15);
                                }
                                if (gelen_labirent[k, l + 1] < sayac && gelen_labirent[k, l + 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l + 1) * 32 + 15);
                                }
                            }
                            else
                            {
                                fare_yon.Y -= 1; fare_donus = 3.2f;
                            }
                        }

                        if (fare_yon.X < (l * 32 + 15))
                        {
                            if (Math.Sqrt(Math.Pow((k * 32 + 15) - fare_yon.Y, 2) + Math.Pow(fare_yon.X - (l * 32 + 15), 2)) > 32)
                            {
                                if (gelen_labirent[k - 1, l] < sayac && gelen_labirent[k - 1, l] != 0)
                                {
                                    fare_yon.Y = ((k - 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k + 1, l] < sayac && gelen_labirent[k + 1, l] != 0)
                                {
                                    fare_yon.Y = ((k + 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k, l - 1] < sayac && gelen_labirent[k, l - 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l - 1) * 32 + 15);
                                }
                                if (gelen_labirent[k, l + 1] < sayac && gelen_labirent[k, l + 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l + 1) * 32 + 15);
                                }
                            }
                            else
                            {
                                fare_yon.X += 1; fare_donus = 4.8f;
                            }
                        }
                        if (fare_yon.X > (l * 32 + 15))
                        {
                            if (Math.Sqrt(Math.Pow((k * 32 + 15) - fare_yon.Y, 2) + Math.Pow(fare_yon.X - (l * 32 + 15), 2)) > 32)
                            {
                                if (gelen_labirent[k - 1, l] < sayac && gelen_labirent[k - 1, l] != 0)
                                {
                                    fare_yon.Y = ((k - 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k + 1, l] < sayac && gelen_labirent[k + 1, l] != 0)
                                {
                                    fare_yon.Y = ((k + 1) * 32 + 15);
                                    fare_yon.X = (l * 32 + 15);
                                }
                                if (gelen_labirent[k, l - 1] < sayac && gelen_labirent[k, l - 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l - 1) * 32 + 15);
                                }
                                if (gelen_labirent[k, l + 1] < sayac && gelen_labirent[k, l + 1] != 0)
                                {
                                    fare_yon.Y = (k * 32 + 15);
                                    fare_yon.X = ((l + 1) * 32 + 15);
                                }
                            }
                            else
                            {
                                fare_yon.X -= 1; fare_donus = 1.6f;
                            }
                        }


                        if (fare_yon.X == (l * 32 + 15) && fare_yon.Y == (k * 32 + 15))
                        {
                            ++sayac;
                        }
                    }

                }
            }
        }
    }
}
