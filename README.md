## Key Components
- Player:
Networked Movement: Players can move and jump, with animations synchronized across the network.
Interaction: Players can kick the ball and collide with other game elements.
Animator: Custom animations for player being idle and running.

- Ball:
Networked Object: Spawns on the server and interacts with players.
Physics: Responds to kicks and collisions, with synchronized movement.

- ScoreManager:
Network Variables: Manages and synchronizes scores for host and client.
UI Updates: Displays scores using TextMeshPro.

- AudioManager:
Sound Effects: Manages playback of various sound effects, including kick, bounce, walk, whistle, cheer, boo, and top post sounds.
