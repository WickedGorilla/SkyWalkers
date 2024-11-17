using Cysharp.Threading.Tasks;
using Game.Environment;
using Game.Infrastructure;
using Infrastructure.Telegram;
using Player;
using UI.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace UI.Views
{
    public class MainMenuController : ViewController<MainMenuView>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WalletService _walletService;
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly TelegramLauncher _telegramLauncher;

        private bool _isLoadPicture;

        public MainMenuController(MainMenuView view,
            IGameStateMachine gameStateMachine,
            WalletService walletService,
            IEnvironmentHolder environmentHolder,
            TelegramLauncher telegramLauncher) : base(view)
        {
            _gameStateMachine = gameStateMachine;
            _walletService = walletService;
            _environmentHolder = environmentHolder;
            _telegramLauncher = telegramLauncher;
        }

        protected override void OnShow()
        {
            View.PlayButton.onClick.AddListener(OnClickPlay);
            View.Initialize(_walletService.Coins, _telegramLauncher.UserName);
            _walletService.Coins.OnChangeValue += OnChangeCoins;

            if (!_isLoadPicture)
            {
                _isLoadPicture = true;
                LoadAndSetPicture();
            }
        }

        protected override void OnHide()
        {
            View.PlayButton.onClick.RemoveListener(OnClickPlay);
            _walletService.Coins.OnChangeValue -= OnChangeCoins;
        }

        private void OnChangeCoins(int coins) 
            => View.UpdateCoins(coins);

        private void OnClickPlay()
        {
            if (_environmentHolder.Environment.Animated)
                return;

            _gameStateMachine.Enter<GameTapLoopState>();
        }

        private async void LoadAndSetPicture()
        {
            return;

            if (string.IsNullOrEmpty(_telegramLauncher.PhotoUrl))
                return;

            var picture = await LoadPicture(_telegramLauncher.PhotoUrl);
            View.SetPicture(picture);
        }

        private async UniTask<Sprite> LoadPicture(string url)
        {
            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
            var operation = uwr.SendWebRequest();

            while (!operation.isDone)
                await UniTask.NextFrame();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error load picture : {uwr.error}");
                return null;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            return ConvertTextureToSprite(texture);
        }

        private Sprite ConvertTextureToSprite(Texture2D texture)
            => Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
    }
}