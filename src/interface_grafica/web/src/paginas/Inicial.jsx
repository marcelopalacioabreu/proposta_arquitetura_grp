import React from 'react'
import { Link } from 'react-router-dom'
import BarraNavegacao from '../componentes/Layout/BarraNavegacao'
import Rodape from '../componentes/Layout/Rodape'
export default function Inicial(){
  return (
    <div className="d-flex flex-column min-vh-100">
      <BarraNavegacao brand="Painel" />
      <main className="flex-fill">
        <section style={{position:'relative', overflow:'hidden'}} className="py-5">
          {/* Abstract SVG background */}
          <div style={{position:'absolute', inset:0, pointerEvents:'none', opacity:0.08}}>
            <svg width="100%" height="100%" viewBox="0 0 800 400" preserveAspectRatio="xMidYMid slice">
              <defs>
                <linearGradient id="g" x1="0" x2="1">
                  <stop offset="0%" stopColor="#6a11cb" />
                  <stop offset="100%" stopColor="#2575fc" />
                </linearGradient>
              </defs>
              <g transform="translate(0,0)">
                <path d="M0 120 C120 200 220 20 360 80 C500 140 620 300 800 180 L800 400 L0 400 Z" fill="url(#g)"/>
                <circle cx="640" cy="60" r="80" fill="#ffd166" />
              </g>
            </svg>
          </div>

          <div className="container" style={{position:'relative'}}>
            <div className="row align-items-center">
              <div className="col-md-7">
                <h1 className="display-5">Prova de conceito — GRP</h1>
                <p className="lead text-muted">Uma interface genérica e baseada em metadados para demonstrar a viabilidade da abordagem proposta para o GRP.</p>
                <div className="mb-3">
                  <Link to="/autenticacao" className="btn btn-primary me-2">Entrar</Link>
                  {/*
                  <Link to="/cadastro" className="btn btn-outline-primary">Criar conta</Link>
                  */}
                </div>
                <small className="text-muted">Esta é uma página pública de demonstração.</small>
              </div>
              <div className="col-md-5 text-center d-none d-md-block">
                <div style={{width:300, height:220, margin:'0 auto', borderRadius:16, background:'linear-gradient(135deg,#6a11cb66,#2575fc66)', display:'flex', alignItems:'center', justifyContent:'center', color:'#fff'}}>
                  <div>
                    <div style={{fontSize:48, fontWeight:700}}>POC</div>
                    <div style={{opacity:0.9}}>Prova de conceito</div>
                  </div>
                </div>
              </div>
            </div>

            <div className="row mt-5">
              <div className="col-md-4">
                <div className="card shadow-sm h-100">
                  <div className="card-body">
                    <h5 className="card-title">Metadados</h5>
                    <p className="card-text text-muted">Telas e formulários são gerados a partir de JSONs de metadados, permitindo mudanças sem alterações no frontend e independência real da tecnologia de apresentação.</p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mt-3 mt-md-0">
                <div className="card shadow-sm h-100">
                  <div className="card-body">
                    <h5 className="card-title">Segurança</h5>
                    <p className="card-text text-muted">Perfis e permissões centralizados, especificação compatível com as especificações NIST/RBAC.</p>
                  </div>
                </div>
              </div>
              <div className="col-md-4 mt-3 mt-md-0">
                <div className="card shadow-sm h-100">
                  <div className="card-body">
                    <h5 className="card-title">Multilocatário</h5>
                    <p className="card-text text-muted">Suporte multi-locatário com isolamento de dados e personalização por cliente. Estrutura hierárquica de organizações, unidades e setores, compatível com a maioria dos cenários corporativos.</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </main>
      <Rodape />
    </div>
  )
}
