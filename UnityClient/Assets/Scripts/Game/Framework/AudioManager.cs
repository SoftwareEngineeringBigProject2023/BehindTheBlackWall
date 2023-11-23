using UnityEngine;

namespace Game.Framework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private SLogger _logger = new SLogger("AudioManager");

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="fileName">音效文件</param>
        /// <param name="volume">音效大小 0~1</param>
        public void PlayEffect(string fileName, float volume = 1)
        {
            _logger.LogInfo($"Play {fileName}");
        }
        
        /// <summary>
        /// 在指定位置播放音效
        /// </summary>
        /// <param name="fileName">音效文件</param>
        /// <param name="volume">音效大小 0~1</param>
        /// <param name="position">播放位置</param>
        public void PlayEffectAt(string fileName, Vector3 position, float volume = 1)
        {
            _logger.LogInfo($"Play {fileName} at {position}");
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="fileName">音效文件</param>
        /// <param name="volume">音效大小 0~1</param>
        /// <param name="loop">循环播放</param>
        public void PlayMusic(string fileName, float volume = 1, bool loop = true)
        {
            _logger.LogInfo($"Play {fileName} loop:{loop}");
        }
        
        /// <summary>
        /// 暂停播放音乐
        /// </summary>
        public void PauseMusic()
        {
            _logger.LogInfo($"Pause");
        }
        
        /// <summary>
        /// 继续播放音乐
        /// </summary>
        public void ResumeMusic()
        {
            _logger.LogInfo($"Resume");
        }

        /// <summary>
        /// 停止播放所有音效音乐
        /// </summary>
        public void StopAll()
        {
            _logger.LogInfo($"Stop All");
        }
    }
}