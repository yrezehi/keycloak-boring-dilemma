# keycloak-boring-dilemma

Dilemma: How can we implement Single Sign-On (SSO) in the most efficient way and enable lazy developer mode, considering the scenario where four applications coexist in the same environment and users employ various identifiers to log in to these applications?

<h1 align="center">
  <br>
  <img src="problem-1.png" alt="problem" width="800">
  <br>
</h1>

Goal: Implement Single Sing-On (SSO) with least amout of work possible.

## Third Party Authenticator

<h1 align="center">
  <br>
  <img src="3rd-party-auth.png" alt="problem" width="800">
  <br>
</h1>

## Proposal

### - First Proposal

Keep claims as is in their applications and introduce a middleware to map the claims to the unified identity

