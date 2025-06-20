// using Content.Shared.Chat;
// using Content.Shared.Corvax.CCCVars;
// using Content.Shared.Corvax.TTS;
// using Content.Shared.DeadSpace.CCCCVars;
// using Content.Shared.DeadSpace.Languages.Components;
// using Content.Shared.Mind;
// using Content.Shared.Mind.Components;
// using Robust.Client.Audio;
// using Robust.Client.ResourceManagement;
// using Robust.Shared.Audio;
// using Robust.Shared.Audio.Systems;
// using Robust.Shared.Configuration;
// using Robust.Shared.ContentPack;
// using Robust.Shared.Utility;
// using Robust.Shared.Player;

// namespace Content.Client.Corvax.TTS;

// /// <summary>
// /// Plays TTS audio in world
// /// </summary>
// // ReSharper disable once InconsistentNaming
// public sealed class TTSSystem : EntitySystem
// {
//     [Dependency] private readonly IConfigurationManager _cfg = default!;
//     [Dependency] private readonly IResourceManager _res = default!;
//     [Dependency] private readonly AudioSystem _audio = default!;
//     private ISawmill _sawmill = default!;
//     private readonly MemoryContentRoot _contentRoot = new();
//     private static readonly ResPath Prefix = ResPath.Root / "TTS";

//     /// <summary>
//     /// Reducing the volume of the TTS when whispering. Will be converted to logarithm.
//     /// </summary>
//     private const float WhisperFade = 3f;

//     /// <summary>
//     /// The volume at which the TTS sound will not be heard.
//     /// </summary>
//     private const float MinimalVolume = -6f;

//     private float _volume = 0.0f;
//     private float _volumeRadio = 0.0f;
//     private bool _playRadio = true;
//     private int _fileIdx = 0;

//     public override void Initialize()
//     {
//         _sawmill = Logger.GetSawmill("tts");
//         _res.AddRoot(Prefix, _contentRoot);
//         _cfg.OnValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged, true);
//         _cfg.OnValueChanged(CCCCVars.TTSVolumeRadio, OnTtsRadioVolumeChanged, true);
//         _cfg.OnValueChanged(CCCCVars.RadioTTSSoundsEnabled, OnTtsPlayRadioChanged, true);
//         SubscribeNetworkEvent<PlayTTSEvent>(OnPlayTTS);
//     }

//     public override void Shutdown()
//     {
//         base.Shutdown();
//         _cfg.UnsubValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged);
//         _cfg.UnsubValueChanged(CCCCVars.TTSVolumeRadio, OnTtsRadioVolumeChanged);
//         _cfg.UnsubValueChanged(CCCCVars.RadioTTSSoundsEnabled, OnTtsPlayRadioChanged);
//         _contentRoot.Dispose();
//     }

//     public void RequestPreviewTTS(string voiceId)
//     {
//         RaiseNetworkEvent(new RequestPreviewTTSEvent(voiceId));
//     }

//     private void OnTtsVolumeChanged(float volume)
//     {
//         _volume = volume;
//     }

//     private void OnTtsRadioVolumeChanged(float volume)
//     {
//         _volumeRadio = volume;
//     }
//     private void OnTtsPlayRadioChanged(bool radio)
//     {
//         _playRadio = radio;
//     }

//     private void OnPlayTTS(PlayTTSEvent ev)
//     {
//         if (ev.IsRadio && !_playRadio)
//         {
//             return;
//         }

//         if (ev.Data == null || ev.Data.Length == 0)
//         {
//             _sawmill.Error("Данные TTS пустые или отсутствуют");
//             return;
//         }

//         Log.Debug($"[TTSSystem] TTS играет.");

//         _sawmill.Verbose($"Play TTS audio {ev.Data.Length} bytes from {ev.SourceUid} entity");

//         var filePath = new ResPath($"{_fileIdx++}.ogg");
//         _contentRoot.AddOrUpdateFile(filePath, ev.Data);


//         var lexiconFilePath = new ResPath($"{_fileIdx + 2}.ogg");

//         if (ev.LexiconData != null)
//             _contentRoot.AddOrUpdateFile(lexiconFilePath, ev.LexiconData);
//         else
//             _contentRoot.AddOrUpdateFile(lexiconFilePath, ev.Data);

//         var audioResource = new AudioResource();
//         audioResource.Load(IoCManager.Instance!, Prefix / filePath);

//         var audioParams = AudioParams.Default
//         .WithVolume(AdjustVolume(ev.IsWhisper, ev.IsRadio))
//         .WithMaxDistance(AdjustDistance(ev.IsWhisper));

//         var soundSpecifier = new ResolvedPathSpecifier(Prefix / filePath);
//         var lexiconSoundSpecifier = new ResolvedPathSpecifier(Prefix / lexiconFilePath);

//         var query = EntityQueryEnumerator<MindContainerComponent>();
//         var sourceUid = GetEntity(ev.SourceUid);

//         Filter entityFilterWithLanguage = Filter.Empty();
//         Filter entityFilterWithoutLanguage = Filter.Empty();

//         if (sourceUid != null && ev.LanguageId != null)
//         {
//             while (query.MoveNext(out var entity, out var mindContainerComp))
//             {
//                 if (TryComp<MindComponent>(mindContainerComp.Mind, out var currentMind) && currentMind.Session != null)
//                 {
//                     if (TryComp<LanguageComponent>(entity, out var lanquage))
//                     {
//                         if (lanquage.LanguagesId.Contains(ev.LanguageId))
//                         {
//                             entityFilterWithLanguage.AddPlayer(currentMind.Session);
//                             Log.Debug($"[TTSSystem] {entity} добавлен в фильтр знающих язык.");
//                         }
//                         else
//                         {
//                             entityFilterWithoutLanguage.AddPlayer(currentMind.Session);
//                             Log.Debug($"[TTSSystem] {entity} добавлен в фильтр не знающих язык.");
//                         }
//                     }
//                     else
//                     {
//                         entityFilterWithLanguage.AddPlayer(currentMind.Session);
//                         Log.Debug($"[TTSSystem] {entity} добавлен в фильтр знающих язык.");
//                     }
//                 }
//             }
//         }

//         if (sourceUid != null)
//         {
//             if (ev.LanguageId == null)
//             {
//                 _audio.PlayEntity(audioResource.AudioStream, sourceUid.Value, soundSpecifier, audioParams);
//                 Log.Debug($"[TTSSystem] null.");
//             }
//             else
//             {
//                 _audio.PlayEntity(soundSpecifier, entityFilterWithLanguage, sourceUid.Value, false, audioParams);
//                 _audio.PlayEntity(lexiconSoundSpecifier, entityFilterWithoutLanguage, sourceUid.Value, false, audioParams);
//                 Log.Debug($"[TTSSystem] {ev.LanguageId}.");
//             }
//         }
//         else
//         {
//             if (ev.LanguageId == null)
//             {
//                 _audio.PlayGlobal(audioResource.AudioStream, soundSpecifier, audioParams);
//                 Log.Debug($"[TTSSystem] global null.");
//             }
//             else
//             {
//                 _audio.PlayGlobal(soundSpecifier, entityFilterWithLanguage, false, audioParams);
//                 _audio.PlayGlobal(lexiconSoundSpecifier, entityFilterWithoutLanguage, false, audioParams);
//                 Log.Debug($"[TTSSystem] global {ev.LanguageId}.");
//             }
//         }

//         _contentRoot.RemoveFile(filePath);
//         _contentRoot.RemoveFile(lexiconFilePath);
//     }

//     private float AdjustVolume(bool isWhisper, bool isRadio)
//     {
//         var volume = MinimalVolume + SharedAudioSystem.GainToVolume(_volume);

//         if (isWhisper)
//         {
//             volume -= SharedAudioSystem.GainToVolume(WhisperFade);
//         }

//         if (isRadio)
//         {
//             volume = MinimalVolume + SharedAudioSystem.GainToVolume(_volumeRadio);
//         }

//         return volume;
//     }

//     private float AdjustDistance(bool isWhisper)
//     {
//         return isWhisper ? SharedChatSystem.WhisperMuffledRange : SharedChatSystem.VoiceRange;
//     }
// }

using Content.Shared.Chat;
using Content.Shared.Corvax.CCCVars;
using Content.Shared.Corvax.TTS;
using Content.Shared.DeadSpace.CCCCVars;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Client.Corvax.TTS;

/// <summary>
/// Plays TTS audio in world
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class TTSSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IResourceManager _res = default!;
    [Dependency] private readonly AudioSystem _audio = default!;

    private ISawmill _sawmill = default!;
    private readonly MemoryContentRoot _contentRoot = new();
    private static readonly ResPath Prefix = ResPath.Root / "TTS";

    /// <summary>
    /// Reducing the volume of the TTS when whispering. Will be converted to logarithm.
    /// </summary>
    private const float WhisperFade = 3f;

    /// <summary>
    /// The volume at which the TTS sound will not be heard.
    /// </summary>
    private const float MinimalVolume = -6f;

    private float _volume = 0.0f;
    private float _volumeRadio = 0.0f;
    private bool _playRadio = true;
    private int _fileIdx = 0;

    public override void Initialize()
    {
        _sawmill = Logger.GetSawmill("tts");
        _res.AddRoot(Prefix, _contentRoot);
        _cfg.OnValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged, true);
        _cfg.OnValueChanged(CCCCVars.TTSVolumeRadio, OnTtsRadioVolumeChanged, true);
        _cfg.OnValueChanged(CCCCVars.RadioTTSSoundsEnabled, OnTtsPlayRadioChanged, true);
        SubscribeNetworkEvent<PlayTTSEvent>(OnPlayTTS);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _cfg.UnsubValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged);
        _cfg.UnsubValueChanged(CCCCVars.TTSVolumeRadio, OnTtsRadioVolumeChanged);
        _cfg.UnsubValueChanged(CCCCVars.RadioTTSSoundsEnabled, OnTtsPlayRadioChanged);
        _contentRoot.Dispose();
    }

    public void RequestPreviewTTS(string voiceId)
    {
        RaiseNetworkEvent(new RequestPreviewTTSEvent(voiceId));
    }

    private void OnTtsVolumeChanged(float volume)
    {
        _volume = volume;
    }

    private void OnTtsRadioVolumeChanged(float volume)
    {
        _volumeRadio = volume;
    }
    private void OnTtsPlayRadioChanged(bool radio)
    {
        _playRadio = radio;
    }

    private void OnPlayTTS(PlayTTSEvent ev)
    {
        if (ev.IsRadio && !_playRadio)
        {
            return;
        }

        _sawmill.Verbose($"Play TTS audio {ev.Data.Length} bytes from {ev.SourceUid} entity");

        var filePath = new ResPath($"{_fileIdx++}.ogg");
        _contentRoot.AddOrUpdateFile(filePath, ev.Data);

        var audioResource = new AudioResource();
        audioResource.Load(IoCManager.Instance!, Prefix / filePath);

        var audioParams = AudioParams.Default
            .WithVolume(AdjustVolume(ev.IsWhisper, ev.IsRadio))
            .WithMaxDistance(AdjustDistance(ev.IsWhisper));

        var soundSpecifier = new ResolvedPathSpecifier(Prefix / filePath);

        if (ev.SourceUid != null)
        {
            var sourceUid = GetEntity(ev.SourceUid.Value);
            _audio.PlayEntity(audioResource.AudioStream, sourceUid, soundSpecifier, audioParams);
        }
        else
        {
            _audio.PlayGlobal(audioResource.AudioStream, soundSpecifier, audioParams);
        }

        _contentRoot.RemoveFile(filePath);
    }

    private float AdjustVolume(bool isWhisper, bool isRadio)
    {
        var volume = MinimalVolume + SharedAudioSystem.GainToVolume(_volume);

        if (isWhisper)
        {
            volume -= SharedAudioSystem.GainToVolume(WhisperFade);
        }

        if (isRadio)
        {
            volume = MinimalVolume + SharedAudioSystem.GainToVolume(_volumeRadio);
        }

        return volume;
    }

    private float AdjustDistance(bool isWhisper)
    {
        return isWhisper ? SharedChatSystem.WhisperMuffledRange : SharedChatSystem.VoiceRange;
    }
}
