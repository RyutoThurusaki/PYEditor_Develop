# PYEditor_Develop
PYEditor開発用プロジェクト  
  
Unityで使えるエディタ拡張やシェーダーの詰め合わせ  
Unity 2019.4.31f1で制作・動作確認

整えられてないプロジェクトなのでまだ未完成だったりバグが残ってたりするのはご愛嬌  
なにかあればissuesにお願いします  
  
# How to use
導入したいプロジェクトに、Githubから直接Cloneします。  
  
  ![CloneSample](https://user-images.githubusercontent.com/41750783/201712861-b7899502-b4a4-4d22-8e48-8c393feb5c37.png)
  
1. GithubDesktopを開き、左上の"File"→"Clone Repository..."をクリック。  
2. URLタブを選択。上の画像と同じウィンドウが出ます。  
3. URLのところに https://github.com/RyutoThurusaki/PYEditor_Develop.git をペースト。  
4. Local Pathには導入したいプロジェクトのAssetsフォルダを指定します。  
5. Cloneボタンをクリック、対象のUnityプロジェクトを開き、ツールバーに"PYEditor"とタブが出ていれば成功です。  
  
SourceTreeなどの場合は自分でやり方調べてください。  
  
更新がある場合は上記でクローンしたRepositoryを管理ツールでFetchしてPullすると最新版になります。

# 完成品  
- InactiveCollidersearcher  
└シーン内のInactiveなコライダーをコンソールに列挙するやーつ
- GlobalRotationResetter  
└オブジェクトsのRotationをグローバル軸でどうこうするやーつ
- MaterialRandamizer  
└オブジェクトsのマテリアルをn個のマテリアルからランダムに設定してくやーつ
- PrefabReplacer  
└配置済みのPrefab_AをPrefab_Bにまとめて置き換えるやーつ
- RotationRandamizer  
└オブジェクトsのRotationをそれぞれ秩序をもってランダムにするやーつ
- TransPadding  
└オブジェクトsの距離感そのままに間隔を広げたり狭めたりするやーつ
- SceneSelector  
└Assets/Scenes 直下のシーンファイルを一覧表示、ワンクリックで開いたりするやーつ

一部は説明書がついてたりついてなかったりします。  
また、拡張内で特筆無い限り基本Undoが実装されています。
  
# Licence
PYEditor  
Copyright (c) 2022 RyutoThurusaki  
  
This software is released under the MIT license.  
https://opensource.org/licenses/MIT

Assets以外のフォルダに入っているアセット/コードに関してはそれぞれのライセンスに準拠します。
