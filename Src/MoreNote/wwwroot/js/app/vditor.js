let vditor;

function initVditor(){
     console.log("vditor init");
     vditor = new Vditor('vditor', {
        // _lutePath: `http://192.168.0.107:9090/lute.min.js?${new Date().getTime()}`,
        // _lutePath: 'src/js/lute/lute.min.js',
        // cdn: "http://localhost:9000",
      
        mode: 'ir',
        
        outline: {
          enable: true,
          position: 'right',
        },
        
        debugger: true,
        typewriterMode: true,
        placeholder: 'Hello, Vditor!',
        preview: {
          markdown: {
            toc: true,
            mark: true,
            footnotes: true,
            autoSpace: true,
          },
          math: {
            engine: 'KaTeX',
          },
        },
        toolbarConfig: {
          pin: true,
        },
        counter: {
          enable: true,
          type: 'text',
        },
       
        
        tab: '\t',
        upload: {
          accept: 'image/*,.mp3, .wav, .rar',
          token: 'test',
          url: '/api/vditor/upload',
          linkToImgUrl: '/api/vditor/fetch',
          filename (name) {
            return name.replace(/[^(a-zA-Z0-9\u4e00-\u9fa5\.)]/g, '').
              replace(/[\?\\/:|<>\*\[\]\(\)\$%\{\}@~]/g, '').
              replace('/\\s/g', '')
          },
        },
    });
}