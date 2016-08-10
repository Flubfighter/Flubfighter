#pragma strict

 public var bounds : Bounds;
 
 private var partSys : ParticleSystem;
 private var particles : ParticleSystem.Particle[];
 
 function Start() {
     partSys = particleSystem;
     particles = new ParticleSystem.Particle[partSys.maxParticles];
 }
 
 function Update() {
     if (partSys.simulationSpace == ParticleSystemSimulationSpace.World) {
         bounds.center = transform.position;  
     }
     var count = partSys.GetParticles(particles);
     for (var i = 0; i < count; i++) {
         if (!bounds.Contains(particles[i].position)) {
             particles[i].lifetime = -1.0;
         }
     }
     partSys.SetParticles(particles, count);
 }