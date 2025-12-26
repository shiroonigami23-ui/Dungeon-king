import wave, struct, math

def create_sfx(filename, duration=0.1, freq=440.0, type='sine'):
    sample_rate = 44100.0
    with wave.open(filename, 'w') as f:
        f.setnchannels(1)
        f.setsampwidth(2)
        f.setframerate(sample_rate)
        for i in range(int(duration * sample_rate)):
            if type == 'sine':
                value = math.sin(2.0 * math.pi * freq * (i / sample_rate))
            elif type == 'noise':
                import random
                value = random.uniform(-1, 1)
            
            # Simple volume envelope (fade out)
            envelope = 1.0 - (i / (duration * sample_rate))
            packed_value = struct.pack('<h', int(value * envelope * 32767))
            f.writeframesraw(packed_value)

# Generate basic sounds for your dungeon
create_sfx('sword_swing.wav', 0.1, 200.0, 'sine') # Low "whoosh"
create_sfx('hit_hurt.wav', 0.05, 800.0, 'noise')  # Static "crunch"
create_sfx('coin_collect.wav', 0.15, 1200.0, 'sine') # High "ding"
create_sfx('dash.wav', 0.1, 150.0, 'sine') # Low "zip"

print("Sounds created! Move these .wav files to your Unity Assets/Audio folder.")