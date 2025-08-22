using System;
//using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TSN.AudioWave;

namespace TSN.FileToVideo
{
    internal static class Utility
    {
        static Utility()
        {
            //_emptyTimeSpan = new TimeSpan();
            _oneSecond = TimeSpan.FromSeconds(1D);
            _oneMillisecond = TimeSpan.FromMilliseconds(1D);
            _oneTick = TimeSpan.FromTicks(1L);
            _amplitude8 = SampleDataType8.MaximumValue;
            _amplitude16 = SampleDataType16.MaximumValue;
            _amplitude32 = SampleDataType32.MaximumValue;
            _allowedAudioToneFrequencies = new List<double> {
                16.35D,
                17.32D,
                18.35D,
                19.45D,
                20.6D,
                21.83D,
                23.12D,
                24.5D,
                25.96D,
                27.5D,
                29.14D,
                30.87D,
                32.7D,
                34.65D,
                36.71D,
                38.89D,
                41.2D,
                43.65D,
                46.25D,
                49D,
                51.91D,
                55D,
                58.27D,
                61.74D,
                65.41D,
                69.3D,
                73.42D,
                77.78D,
                82.41D,
                87.31D,
                92.5D,
                98D,
                103.83D,
                110D,
                116.54D,
                123.47D,
                130.81D,
                138.59D,
                146.83D,
                155.56D,
                164.81D,
                174.61D,
                185D,
                196D,
                207.65D,
                220D,
                233.08D,
                246.94D,
                261.63D,
                277.18D,
                293.66D,
                311.13D,
                329.63D,
                349.23D,
                369.99D,
                392D,
                415.3D,
                440D,
                466.16D,
                493.88D,
                523.25D,
                554.37D,
                587.33D,
                622.25D,
                659.25D,
                698.46D,
                739.99D,
                783.99D,
                830.61D,
                880D,
                932.33D,
                987.77D,
                1046.5D,
                1108.73D,
                1174.66D,
                1244.51D,
                1318.51D,
                1396.91D,
                1479.98D,
                1567.98D,
                1661.22D,
                1760D,
                1864.66D,
                1975.53D,
                2093D,
                2217.46D,
                2349.32D,
                2489.02D,
                2637.02D,
                2793.83D,
                2959.96D,
                3135.96D,
                3322.44D,
                3520D,
                3729.31D,
                3951.07D,
                4186.01D,
                4434.92D,
                4698.63D,
                4978.03D,
                5274.04D,
                5587.65D,
                5919.91D,
                6271.93D,
                6644.88D,
                7040D,
                7458.62D,
                7902.13D
            }.AsReadOnly();
        }


        private const string EncryptionPassword1 = "Mustafa Tosun, 1992, The Republic of Turkey";
        private const string EncryptionPassword2 = "www.mustafatosun.net";
        private const string DescriptionFormat = "{0} | Resoulution Width: {1} | Resoulution Height: {2} | Frames per Second: {3} | Pixel Width, Height: {4} | Every pixel presents 3 bytes in order  as RGB. | Bytes were ordered left to right, by starting at top in every frame. | Audio Tone Frequency Interval: [1 Hz, 20154 Hz] | Creator and Publisher: Mustafa Tosun (www.mustafatosun.net) | All rights reserved. Any kind of declaring and/or publishing and/or keeping/saving the real contents of this encrypted file is not allowed. If you read this text, congratulations to you! You must have spent a lot of time to decrypt it. Destroy everything related this file, immediately.";
        //private const ulong MinAudioToneFrequency = 1UL;
        //private const ulong MaxAudioToneFrequency = 20154UL;
        //private const ulong AudioToneFrequencyIntervalLength = MaxAudioToneFrequency - MinAudioToneFrequency;

        private static readonly IReadOnlyList<double> _allowedAudioToneFrequencies;
        //private static readonly TimeSpan _emptyTimeSpan;
        private static readonly TimeSpan _oneSecond;
        private static readonly TimeSpan _oneMillisecond;
        private static readonly TimeSpan _oneTick;
        private static readonly SampleDataType8 _amplitude8;
        private static readonly SampleDataType16 _amplitude16;
        private static readonly SampleDataType32 _amplitude32;

        public static event EventHandler<CalculatedFileEventArgs> CalculatedFile;
        public static event EventHandler<ProgressChangedEventArgs> SavedImageFrame;
        public static event EventHandler<CancellableEventArgs> SavedAllImageFrames;
        public static event EventHandler<ProgressChangedEventArgs> CheckedFrame;
        public static event EventHandler<CancellableEventArgs> CheckedAllFrames;
        public static event EventHandler<ProgressChangedEventArgs> SavedAudioFrame;
        public static event EventHandler<CancellableEventArgs> SavedAllAudioFrames;


        private static IEnumerable<long> GetFibonacciSeries(long lowerBound, long upperBound)
        {
            if (upperBound < 1L)
                yield break;
            for (long temp, prev = 0L, fib = 1L; fib <= upperBound; prev = temp)
            {
                if (lowerBound <= fib)
                    yield return fib;
                temp = fib;
                fib = checked(prev + fib);
            }
        }
        private static IEnumerable<long> GetFibonacciSeries(long lowerBound, ushort count)
        {
            var c = (int)count;
            if (c > 0)
            {
                var tmp1 = 1L;
                yield return tmp1;
                if (c > 1)
                {
                    var tmp2 = 1L;
                    yield return tmp2;
                    long fib;
                    for (int i = 2; i < c;)
                    {
                        fib = checked(tmp1 + tmp2);
                        tmp1 = tmp2;
                        tmp2 = fib;
                        if (lowerBound <= fib)
                        {
                            yield return fib;
                            i++;
                        }
                    }
                }
            }
        }
        private static decimal SelectFromInterval(decimal value, decimal currentLowerLimit, decimal currentUpperLimit, decimal lowerLimit, decimal upperLimit) => (value - currentLowerLimit) / (currentUpperLimit - currentLowerLimit) * (upperLimit - lowerLimit) + lowerLimit;
        private static byte[] Encrypt(byte[] input, string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                for (int i = 0; i < 10; i++)
                    rng.GetBytes(salt);
            var AES = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7
            };
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = CipherMode.CFB;
            using (var ms = new MemoryStream())
            {
                ms.Write(salt, 0, salt.Length);
                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(input, 0, input.Length);
                    cs.Flush();
                    ms.Flush();
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
        }
        private static byte[] GetEncryptedBuffer(string sourceFile, string description)
        {
            byte[] buffer;
            using (var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = (byte)fs.ReadByte();
            }
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, (description, buffer));
                ms.Flush();
                ms.Position = 0;
                buffer = Encrypt(ms.ToArray(), EncryptionPassword1);
            }
            GC.Collect();
            return buffer;
        }
        private static T[] TransformTo<T>(byte[] bytes, int grouplength, Func<byte[], T> transformer)
        {
            int div = Math.DivRem(bytes.Length, grouplength, out var rem), x = 0;
            var transforms = new T[div + (rem == 0 ? 0 : 1)];
            for (int i = 0; i < div; i++)
            {
                var k = i * grouplength;
                var arr = new byte[grouplength];
                for (int j = 0; j < grouplength; j++)
                    arr[j] = bytes[k + j];
                transforms[x++] = transformer(arr);
            }
            if (rem > 0)
            {
                var k = div * grouplength;
                var arr = new byte[grouplength];
                for (int j = 0; j < rem; j++)
                    arr[j] = bytes[k + j];
                transforms[x] = transformer(arr);
            }
            return transforms;

        }
        private static Color[] TransformToColors(byte[] bytes) => TransformTo(bytes, 3, arr => Color.FromArgb(arr[0], arr[1], arr[2]));
        //private static double[] TransformToAudioTones(byte[] bytes) => TransformTo(bytes, 8, arr => BitConverter.ToUInt32(arr, 0)).Select(d => _allowedAudioToneFrequencies[(int)Math.Round(checked(d * (_allowedAudioToneFrequencies.Count - 1) / (decimal)uint.MaxValue), 0)]).ToArray();
        private static double[] TransformToAudioTones(byte[] bytes) => TransformTo(bytes, 8, arr => BitConverter.ToUInt32(arr, 0)).Select(d => _allowedAudioToneFrequencies[(int)Math.Round(SelectFromInterval(d, uint.MinValue, uint.MaxValue, 0M, _allowedAudioToneFrequencies.Count - 1), 0)]).ToArray();
        private static Bitmap GetCanvas(byte[] bytes, Size pixelSize, int resolutionWidth, int resolutionHeight)
        {
            var colors = TransformToColors(bytes);
            var canvas = new Bitmap(resolutionWidth, resolutionHeight);
            using (var gr = Graphics.FromImage(canvas))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.FillRectangle(Brushes.White, new Rectangle(Point.Empty, canvas.Size));
                for (int i = 0; i < colors.Length; i++)
                {
                    var y = Math.DivRem(i, resolutionWidth / pixelSize.Width, out var x);
                    using (var br = new SolidBrush(colors[i]))
                        gr.FillRectangle(br, new Rectangle(new Point(x * pixelSize.Width, y * pixelSize.Height), pixelSize));
                }
            }
            return canvas;
        }
        public static void FileToVideo(string sourceFile, string outputDirectory, string description, Size pixelSize, int resolutionWidth = 3840, int resolutionHeight = 2160, int framesPerSecond = 60, byte bitsPerSample = 32, ushort channels = 2, SamplingFrequencies samplePerSec = SamplingFrequencies.Hz96000)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));
            if (framesPerSecond < 0)
                throw new ArgumentOutOfRangeException(nameof(framesPerSecond));
            var buffer = GetEncryptedBuffer(sourceFile, string.Format(DescriptionFormat, description, resolutionWidth, resolutionHeight, framesPerSecond, pixelSize));
            var _bytesPerFrame = 3 * resolutionWidth / pixelSize.Width * (resolutionHeight / pixelSize.Height);
            var div = Math.DivRem(buffer.Length, _bytesPerFrame, out var rem);
            var framesCount = div + (rem == 0 ? 0 : 1);
            var framesLength = TimeSpan.FromSeconds((double)framesCount / framesPerSecond);
            var ticksPerFrame = (decimal)_oneSecond.Ticks / framesPerSecond;
            var frameDuration = TimeSpan.FromTicks((long)ticksPerFrame);
            #region . OLD | Fibonacci .
            //var fibonacci = GetFibonacciSeries(_oneMillisecond.Ticks, framesLength.Ticks).Reverse().ToArray();
            //if (fibonacci.Length != framesCount)
            //    if (fibonacci.Length > framesCount)
            //        fibonacci = fibonacci.Take(framesCount).ToArray();
            //    else
            //    {
            //        var fibDiv = Math.DivRem(framesCount, fibonacci.Length, out var fibRem);
            //        var expanded = new long[framesCount];
            //        for (int i = 0, k = 0; i < fibonacci.Length; i++)
            //            for (int j = 0, c = fibDiv + (i < fibRem ? 1 : 0); j < c; j++)
            //                expanded[k++] = fibonacci[i];
            //        fibonacci = expanded;
            //    }
            #endregion
            var e1 = new CalculatedFileEventArgs(buffer.Length, framesCount, framesLength);
            CalculatedFile?.Invoke(null, e1);
            if (e1.Cancel)
                return;
            Directory.CreateDirectory(outputDirectory);
            var imagesDir = Directory.CreateDirectory(Path.Combine(outputDirectory, $"Image Frames ({framesPerSecond} fps)")).FullName;
            var cancel = false;
            //var allTones = new (double Frequency, TimeSpan Starting, TimeSpan WaveLength)[framesCount][];
            byte[] bytes = null;
            var allFrequencies = new double[framesCount][];
            var pResult = Parallel.For(0, div, (i, state) => {
                if (state.ShouldExitCurrentIteration || state.IsStopped)
                    return;
                if (state.IsExceptional)
                {
                    cancel = true;
                    state.Stop();
                    return;
                }
                Array.Copy(buffer, i * _bytesPerFrame, bytes = new byte[Math.Min(_bytesPerFrame, buffer.Length)], 0, bytes.Length);
                using (var canvas = GetCanvas(bytes, pixelSize, resolutionWidth, resolutionHeight))
                    canvas.Save(Path.Combine(imagesDir, $"{i}.png"), ImageFormat.Png);
                allFrequencies[i] = TransformToAudioTones(bytes);
                var e2 = new ProgressChangedEventArgs(i, framesCount);
                SavedImageFrame?.Invoke(null, e2);
                if (e2.Cancel)
                {
                    cancel = true;
                    state.Stop();
                }
            });
            if (cancel)
                return;
            if (rem != 0)
            {
                Array.Copy(buffer, div * _bytesPerFrame, bytes = new byte[rem], 0, rem);
                using (var canvas = GetCanvas(bytes, pixelSize, resolutionWidth, resolutionHeight))
                    canvas.Save(Path.Combine(imagesDir, $"{div}.png"), ImageFormat.Png);
                allFrequencies[div] = TransformToAudioTones(bytes);
                SavedImageFrame?.Invoke(null, new ProgressChangedEventArgs(div, framesCount));
            }
            GC.SuppressFinalize(bytes);
            bytes = null;
            GC.Collect();
            var e3 = new CancellableEventArgs();
            SavedAllImageFrames?.Invoke(null, e3);
            if (e3.Cancel)
                return;
            var frequencies = new double[allFrequencies.GetLength(0)];
            pResult = Parallel.For(0, frequencies.Length, (i, state) =>
            {
                if (state.ShouldExitCurrentIteration || state.IsStopped)
                    return;
                if (state.IsExceptional)
                {
                    cancel = true;
                    state.Stop();
                    return;
                }
                frequencies[i] = allFrequencies[i].Average();
                var e2 = new ProgressChangedEventArgs(i, framesCount);
                CheckedFrame?.Invoke(null, e2);
                if (e2.Cancel)
                {
                    cancel = true;
                    state.Stop();
                }
            });
            GC.SuppressFinalize(allFrequencies);
            allFrequencies = null;
            GC.Collect();
            CheckedAllFrames?.Invoke(null, new CancellableEventArgs());
            var audiosDir = Directory.CreateDirectory(Path.Combine(outputDirectory, $"Audio Frames ({framesPerSecond} fps)")).FullName;
            //Action<double[], TimeSpan, int> waveGenerator = null;
            Func<dynamic> waveInstantiator = null;
            Action<dynamic, double[], TimeSpan> waveAdder = null;
            Action<dynamic, int> waveSaver = (wave, x) => {
                using (var fs = new FileStream(Path.Combine(audiosDir, $"{x}.wav"), FileMode.Create, FileAccess.Write))
                {
                    wave.Save(fs);
                    fs.Flush();
                }
            };
            switch (bitsPerSample <= 8 ? 8 : (bitsPerSample >= 32 ? 32 : (bitsPerSample < 16 ? ((16 - bitsPerSample) <= (bitsPerSample - 8) ? 16 : 8) : (bitsPerSample > 16 ? ((32 - bitsPerSample) < (bitsPerSample - 16) ? 32 : 16) : 16))))
            {
                case 8:
                    waveInstantiator = () => new AudioWave8Bit(channels, samplePerSec);
                    waveAdder = (wave, tones, duration) => {
                        var tones_ = tones.Select(f => new AudioTone<SampleDataType8>(_amplitude8, f)).ToArray();
                        wave.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType8>(Enumerable.Range(0, channels).Select(i => tones_).ToArray()), duration);
                    };
                    //waveGenerator = (tones, duration, x) => {
                    //    //var tones_ = tones.Select(t => new AudioTone<SampleDataType8>(_amplitude8, t.Frequency, t.Starting, t.WaveLength)).ToArray();
                    //    var tones_ = tones.Select(f => new AudioTone<SampleDataType8>(_amplitude8, f)).ToArray();
                    //    var wave = new AudioWave8Bit(channels, samplePerSec);
                    //    wave.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType8>(Enumerable.Range(0, channels).Select(i => tones_).ToArray()), duration);
                    //    using (var fs = new FileStream(Path.Combine(audiosDir, $"{x}.wav"), FileMode.Create, FileAccess.Write))
                    //    {
                    //        wave.Save(fs);
                    //        fs.Flush();
                    //    }
                    //};
                    break;
                case 16:
                    waveInstantiator = () => new AudioWave16Bit(channels, samplePerSec);
                    waveAdder = (wave, tones, duration) => {
                        var tones_ = tones.Select(f => new AudioTone<SampleDataType16>(_amplitude16, f)).ToArray();
                        wave.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType16>(Enumerable.Range(0, channels).Select(i => tones_).ToArray()), duration);
                    };
                    break;
                case 32:
                    waveInstantiator = () => new AudioWave32Bit(channels, samplePerSec);
                    waveAdder = (wave, tones, duration) => {
                        var tones_ = tones.Select(f => new AudioTone<SampleDataType32>(_amplitude32, f)).ToArray();
                        wave.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType32>(Enumerable.Range(0, channels).Select(i => tones_).ToArray()), duration);
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bitsPerSample));
            }
            pResult = Parallel.For(0, div, (i, state) => {
                if (state.ShouldExitCurrentIteration || state.IsStopped)
                    return;
                if (state.IsExceptional)
                {
                    cancel = true;
                    state.Stop();
                    return;
                }
                ////var ticksPerTone = ticksPerFrame / allFrequencies[i].Length;
                ////waveGenerator(allFrequencies[i].Select((f, j) => (f, TimeSpan.FromTicks((long)(ticksPerTone * j)), TimeSpan.FromTicks((long)(ticksPerTone * (allFrequencies[i].Length - j))))).ToArray(), frameDuration, i);
                //waveGenerator(new[] { frequencies[i] }, frameDuration, i);
                //var wave = waveInstantiator();
                //if (ticksPerFrame >= allFrequencies[i].Length)
                //{
                //    var duration = TimeSpan.FromTicks((long)(ticksPerFrame / allFrequencies[i].Length));
                //    waveAdder(wave, new[] { allFrequencies[i][0] }, TimeSpan.FromTicks(frameDuration.Ticks - duration.Ticks * (allFrequencies[i].Length - 1)));
                //    for (int j = 1; j < allFrequencies[i].Length; j++)
                //        waveAdder(wave, new[] { allFrequencies[i][j] }, duration);
                //}
                //else
                //{
                //    var div_ = (int)Math.DivRem(allFrequencies[i].Length, frameDuration.Ticks, out var rem_);
                //    for (int j = 0; j < allFrequencies[i].Length; j += div_)
                //        waveAdder(wave, Enumerable.Range(j, div_).Select(k => allFrequencies[i][k]).ToArray(), _oneTick);
                //    waveAdder(wave, Enumerable.Range(div_, (int)rem_).Select(k => allFrequencies[i][k]).ToArray(), _oneTick);
                //}
                //waveSaver(wave, i);
                var wave = waveInstantiator();
                waveAdder(wave, new[] { frequencies[i] }, frameDuration);
                waveSaver(wave, i);
                var e4 = new ProgressChangedEventArgs(i, framesCount);
                SavedAudioFrame?.Invoke(null, e4);
                if (e4.Cancel)
                {
                    cancel = true;
                    state.Stop();
                }
            });
            if (cancel)
                return;
            if (rem != 0)
            {
                //////var ticksPerTone = ticksPerFrame / allFrequencies[div].Length;
                //////waveGenerator(allFrequencies[div].Select((f, j) => (f, TimeSpan.FromTicks((long)(ticksPerTone * j)), TimeSpan.FromTicks((long)(ticksPerTone * (allFrequencies[div].Length - j))))).ToArray(), frameDuration, div);
                ////waveGenerator(new[] { frequencies[div] }, frameDuration, div);
                //var wave = waveInstantiator();
                //if (ticksPerFrame >= allFrequencies[div].Length)
                //{
                //    var duration = TimeSpan.FromTicks((long)(ticksPerFrame / allFrequencies[div].Length));
                //    waveAdder(wave, new[] { allFrequencies[div][0] }, TimeSpan.FromTicks(frameDuration.Ticks - duration.Ticks * (allFrequencies[div].Length - 1)));
                //    for (int j = 1; j < allFrequencies[div].Length; j++)
                //        waveAdder(wave, new[] { allFrequencies[div][j] }, duration);
                //}
                //else
                //{
                //    var div_ = (int)Math.DivRem(allFrequencies[div].Length, frameDuration.Ticks, out var rem_);
                //    for (int j = 0; j < allFrequencies[div].Length; j += div_)
                //        waveAdder(wave, Enumerable.Range(j, div_).Select(k => allFrequencies[div][k]).ToArray(), _oneTick);
                //    waveAdder(wave, Enumerable.Range(div_, (int)rem_).Select(k => allFrequencies[div][k]).ToArray(), _oneTick);
                //}
                //waveSaver(wave, div);
                var wave = waveInstantiator();
                waveAdder(wave, new[] { frequencies[div] }, frameDuration);
                waveSaver(wave, div);
                SavedAudioFrame?.Invoke(null, new ProgressChangedEventArgs(div, framesCount));
            }
            //GC.SuppressFinalize(allFrequencies);
            //allFrequencies = null;
            GC.SuppressFinalize(frequencies);
            frequencies = null;
            GC.Collect();
            SavedAllAudioFrames?.Invoke(null, new CancellableEventArgs());
            #region . OLD .
            //if (e4.Cancel)
            //    return;
            //IList tones;
            //Func<int, IEnumerable> tonesGenerator;
            //Func<IList, dynamic> waveGenerator;
            //switch (bits)
            //{
            //    case 8:

            //        tones = new List<AudioTone<SampleDataType8>>(framesCount);
            //        tonesGenerator = i => allTones[i].Select(d => {
            //            var starting = TimeSpan.FromTicks((long)(ticksPerFrame * i));
            //            var length = TimeSpan.FromTicks(fibonacci[i]);
            //            var maxLength = framesLength - starting;
            //            return new AudioTone<SampleDataType8>(_amplitude8, d, starting, length > maxLength ? maxLength : length);
            //        });
            //        waveGenerator = list => {
            //            var l = ((List<AudioTone<SampleDataType8>>)list).AsReadOnly();
            //            var wave8 = new AudioWave8Bit(channels, samplePerSec);
            //            wave8.InsertedSample += Wave_InsertedSample;
            //            wave8.InsertedWave += Wave_InsertedWave;
            //            if (!wave8.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType8>(Enumerable.Range(0, channels).Select(x => l.ToArray()).ToArray()), framesLength))
            //                wave8 = null;
            //            return wave8;
            //        };
            //        break;
            //    case 16:
            //        tones = new List<AudioTone<SampleDataType16>>(framesCount);
            //        tonesGenerator = i => allTones[i].Select(d => {
            //            var starting = TimeSpan.FromTicks((long)(ticksPerFrame * i));
            //            var length = TimeSpan.FromTicks(fibonacci[i]);
            //            var maxLength = framesLength - starting;
            //            return new AudioTone<SampleDataType16>(_amplitude16, d, starting, length > maxLength ? maxLength : length);
            //        });
            //        waveGenerator = list => {
            //            var l = ((List<AudioTone<SampleDataType16>>)list).AsReadOnly();
            //            var wave16 = new AudioWave16Bit(channels, samplePerSec);
            //            wave16.InsertedSample += Wave_InsertedSample;
            //            wave16.InsertedWave += Wave_InsertedWave;
            //            if (!wave16.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType16>(Enumerable.Range(0, channels).Select(x => l.ToArray()).ToArray()), framesLength))
            //                wave16 = null;
            //            return wave16;
            //        };
            //        break;
            //    case 32:
            //        tones = new List<AudioTone<SampleDataType32>>(framesCount);
            //        tonesGenerator = i => allTones[i].Select(d => {
            //            var starting = TimeSpan.FromTicks((long)(ticksPerFrame * i));
            //            var length = TimeSpan.FromTicks(fibonacci[i]);
            //            var maxLength = framesLength - starting;
            //            return new AudioTone<SampleDataType32>(_amplitude32, d, starting, length > maxLength ? maxLength : length);
            //        });
            //        waveGenerator = list => {
            //            var l = ((List<AudioTone<SampleDataType32>>)list).AsReadOnly();
            //            var wave32 = new AudioWave32Bit(channels, samplePerSec);
            //            wave32.InsertedSample += Wave_InsertedSample;
            //            wave32.InsertedWave += Wave_InsertedWave;
            //            if (!wave32.AddWave(AudioWaveExampleTypes.Sine, new ChannelCollection<SampleDataType32>(Enumerable.Range(0, channels).Select(x => l.ToArray()).ToArray()), framesLength))
            //                wave32 = null;
            //            return wave32;
            //        };
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(bitsPerSample));
            //}
            //for (int i = 0; i < allTones.Length; i++)
            //{
            //    foreach (var tone in tonesGenerator(i))
            //        tones.Add(tone);
            //    var e4 = new ProgressChangedEventArgs(i, framesCount);
            //    CheckedFrame?.Invoke(null, e4);
            //    if (e4.Cancel)
            //        break;
            //}
            //var e5 = new CancellableEventArgs();
            //CheckedAllFrames?.Invoke(null, e5);
            //if (e5.Cancel)
            //    return;
            //var wave = waveGenerator(tones);
            //if (wave != null)
            //    using (var fs = new FileStream(Path.Combine(outputDirectory, $"audio.wav"), FileMode.Create, FileAccess.Write))
            //    {
            //        wave.Save(fs);
            //        fs.Flush();
            //    }
            //allTones = null;
            //wave = null;
            //GC.Collect();
            #endregion
        }
        public static string EncryptFileName(string fileName) => Base32.Convert(Encrypt(Encoding.UTF8.GetBytes(fileName), EncryptionPassword2));

        //private static void Wave_InsertedSample(object sender, AudioWave.ProgressChangedEventArgs e)
        //{
        //    var e_ = new ProgressChangedEventArgs(e.Index, e.Count);
        //    SavedAudioFrame?.Invoke(sender, e_);
        //    if (e_.Cancel)
        //        e.CancelOperation();
        //}
        //private static void Wave_InsertedWave(object sender, EventArgs e) => SavedAllAudioFrames?.Invoke(sender, new CancellableEventArgs());
    }
}