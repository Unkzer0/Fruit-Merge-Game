<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <title>Fruit Merge Game</title>
  <style>
    body {
      font-family: Arial, sans-serif;
      max-width: 900px;
      margin: 2rem auto;
      padding: 1rem;
      background-color: #fff8f0;
      color: #333;
    }
    h1, h2, h3 {
      color: #de7000;
    }
    code {
      background-color: #eee;
      padding: 2px 4px;
      border-radius: 3px;
      font-size: 0.95em;
    }
    pre {
      background-color: #f4f4f4;
      padding: 10px;
      overflow-x: auto;
    }
    a {
      color: #e66b00;
    }
  </style>
</head>
<body>

  <h1>🍎 Fruit Merge</h1>
  <p><strong>Fruit Merge</strong> is a hyper-casual, physics-based mobile game built in Unity. Players drop and merge identical fruits to create higher-tier fruits and earn points. The game is optimized for mobile with touch controls, audio feedback, scoring, and polished animations.</p>

  <h2>🛠 Features</h2>
  <ul>
    <li>Touch & drag dropper movement</li>
    <li>Drop fruit on release or swipe</li>
    <li>Merge identical fruits into higher-tier fruits</li>
    <li>Score system based on fruit type (configurable)</li>
    <li>High score saving using <code>PlayerPrefs</code></li>
    <li>Bounce animation and sound on merge</li>
    <li>Polygon colliders for accurate physics</li>
    <li>Continuous collision detection to prevent pass-through</li>
  </ul>

  <h2>🎮 Controls</h2>
  <ul>
    <li><strong>Touch & Drag:</strong> Move the dropper left or right</li>
    <li><strong>Release:</strong> Drop the current fruit</li>
    <li><strong>Swipe:</strong> Fast swipe also triggers a drop</li>
  </ul>

  <h2>📁 Project Structure</h2>
  <ul>
    <li><code>Fruit.cs</code> – Handles fruit data and merge logic</li>
    <li><code>FruitSelector.cs</code> – Picks and previews the next fruit</li>
    <li><code>MergeManager.cs</code> – Handles merge logic, scoring, and bounce</li>
    <li><code>FruitDropperController.cs</code> – Controls dropper movement and drop timing</li>
    <li><code>ScoreManager.cs</code> – Manages score and high score UI</li>
  </ul>

  <h2>🧪 Setup Instructions</h2>
  <ol>
    <li>Open the project in Unity 2022.3.64f1</li>
    <li>Make sure <code>LeanTween</code> is installed (via Asset Store or manually)</li>
    <li>Create and assign <code>FruitData</code> ScriptableObjects for each fruit</li>
    <li>Assign correct <code>PolygonCollider2D</code> and set <code>Rigidbody2D.collisionDetectionMode = Continuous</code> for all fruit prefabs</li>
    <li>Set tags and layers for collision detection (e.g. <code>Fruit</code> and <code>Container</code>)</li>
    <li>Configure UI elements for score and next fruit preview</li>
  </ol>

  <h2>📈 Scoring</h2>
  <p>Each fruit has a custom score value stored in its <code>FruitData</code>. When two identical fruits merge, the score is increased by the value of that fruit.</p>

  <h2>📦 Assets Used</h2>
  <ul>
    <li><a href="https://assetstore.unity.com/packages/tools/animation/leantween-3595" target="_blank">LeanTween</a> – lightweight tweening engine</li>
    <li>Custom or royalty-free fruit sprites (not included here)</li>
  </ul>

  <h2>📝 License</h2>
  <p>This project is for educational and prototyping purposes. Please replace sprites and sounds with your own assets before publishing commercially.</p>

</body>
</html>
